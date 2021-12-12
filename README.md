# HUGs
### _**H**elpful and **U**seful **G**enerators of **S**ource_
An opinionated .NET Source Generator to help with the DDD boilerplate code.

## Description
**HUGs** is a **Source Generator** framework that targets **DDD object classes generation**. The Source Generator uses *Additional Files* with DDD object schemas in YAML format to build the DDD model and generate the DDD object classes for **ValueObjects**, **Entities**, **Aggregates**, and **Enumerations**. Moreover, **HUGs** also generates a plain **DbEntity** class that is database-friendly, and it generates a **Mapper** that can map a DDD object to a database object and vice versa. Therefore, **HUGs** can generate up to 3 classes from a single YAML schema.

## Installation
TODO

## Usage

**HUGs** require YAML files with file extension `.dddschema` and `.dddconfig` for the `AdditionalFiles`.

> NOTE: JSON is also a valid YAML format.

Schemas for **Entity**, **Aggregate**, and **ValueObject** DDD object expect the follow structure of the corresponding *AdditionalFile*:

```
Kind: ValueObject | Entity | Aggregate 
Name: <NameOfTheClassHere>
Properties:
  - Name: <NameOfProperty>
    Type: <TypeOfProperty>
    Computed: true | false
```

The `Kind` specifies the type of DDD object. The `Kind` affects the structure of the generated code and the inherited class. The generated class name is specified by the `Name`, and the class properties can be specified as a list in `Properties`. The property requires its `Name` and `Type`. The type supports nullability `?` and `Computed` properties.

> NOTE: The computed properties are not initialized in a constructor. For this purpose, an `OnInitialized()` method is added. The model class and this method are both `partial` to add an initialization to the `Computed` properties or maybe to add a validation.

DDD **Enumeration** requires the following structure:

```
Kind: Enumeration
Name: <Name>
Properties:
  - Name: <EnumPropertyName>
    Type: <EnumPropertyType>
Values:
  - Name: <EnumValueName>
    Properties: 
      <EnumPropertyName>: <EnumPropertyValue>
```

The `Kind`, `Name`, and `Properties` follow the same requirements as for the other DDD object kinds (except for `Computed` in a property specification - the behavior is not defined). The `Values` are used to initialize the enumeration values in the form of `public static readonly` fields. The `Values` require a name of the Enumeration value and values for all of the specified properties in the `Properties` section.

### Type support
- C\# primitive types are supported
- All of the DDD types specified in `.dddschema` files are supported; ID classes as well
- Nullable types are supported with the standard suffix `?`, e.g. `string?`
- Collections are represented with the standard array brackets suffix: `[]`; a string collection can be represented as `Type: string[]`

### Generation Configuration
**HUGs** support a minor configuration for the generated classes by adding an `AdditionalFile` with file extension `.dddconfig`. You can specified namespaces of the generated files for individual DDD object kinds, and also list additional `using` statements for the generated files. The expected structure is the following:

```
TargetNamespaces:
  ValueObject: <NamespaceForGeneratedValueObjects>
  Entity: <NamespaceForGeneratedEntities>
  Aggregate: <NamespaceForGeneratedAggregates>
  Enumeration: <NamespaceForGeneratedEnumerations>
  DbEntity: <NamespaceForGeneratedDbEntities>

AdditionalUsings:
  - <GeneratedFileAdditionalUsing>
  - <GeneratedFileAdditionalUsing>
```

> NOTE: Only **ONE** configuration file is expected!

### Examples

#### ValueObject
`Address` ValueObject schema:

```
Kind: ValueObject
Name: Address
Properties:
  - Name: Street
    Type: string
  - Name: Street2
    Type: string?
  - Name: City
    Type: string
  - Name: Zip
    Type: string
  - Name: CountryId
    Type: CountryId
```

Generates a DDD object class:
```
using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.ValueObject
{
    public partial class Address : HUGs.Generator.DDD.Framework.BaseModels.ValueObject
    {
        public string Street { get; }

        public string? Street2 { get; }

        public string City { get; }

        public string Zip { get; }

        public CountryId CountryId { get; }

        public Address(string Street, string? Street2, string City, string Zip, CountryId CountryId)
        {
            this.Street = Street;
            this.Street2 = Street2;
            this.City = City;
            this.Zip = Zip;
            this.CountryId = CountryId;
            OnInitialized();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street;
            yield return Street2;
            yield return City;
            yield return Zip;
            yield return CountryId;
        }

        partial void OnInitialized();
    }
}
```

A DB entity:

```
using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.DbEntity
{
    public class AddressDbEntity
    {
        public string Street { get; set; }

        public string? Street2 { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public CountryId CountryId { get; set; }

    }
}
```

And a mapper:

```
using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Mapper
{
    public class AddressMapper : DbEntityMapper<Address, AddressDbEntity>
    {
        public AddressMapper(IDbEntityMapperFactory factory)
        	: base(factory)
        {
        }

        public override AddressDbEntity ToDbEntity(Address obj)
        {
            return new AddressDbEntity
            {
            	Street = obj.Street,
            	Street2 = obj.Street2,
            	City = obj.City,
            	Zip = obj.Zip,
            	CountryId = MapDbEntityId(obj.CountryId)
            };
        }

        public override Address ToDddObject(AddressDbEntity obj)
        {
            return new Address
            (
            	obj.Street,
            	obj.Street2,
            	obj.City,
            	obj.Zip,
            	MapDddObjectId(obj.CountryId)
            );
        }
    }
}
```

#### Aggregate and Entitties
Aggregates and Entities are produced similarly; an example for an `Order` Aggregate:

```
Kind: Aggregate
Name: Order
Properties:
  - Name: Number
    Type: string
  - Name: CreatedDate
    Type: DateTime
  - Name: Items
    Type: OrderItem[]
  - Name: ShippingAddress
    Type: Address
  - Name: TotalPrice
    Type: decimal
    Computed: true
  - Name: State
    Type: OrderState
```

A DDD object class:

```
using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Aggregate
{
    public class OrderId : EntityId<Order>
    {
        public OrderId(Guid value)
        	: base(value)
        {
        }
    }

    public partial class Order : HUGs.Generator.DDD.Framework.BaseModels.Aggregate<OrderId>
    {
        private List<OrderItem> _Items;

        public string Number { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public IReadOnlyList<OrderItem> Items => _Items;

        public Address ShippingAddress { get; private set; }

        public decimal TotalPrice { get; private set; }

        public OrderState State { get; private set; }

        public Order(IId<OrderId> id, string Number, DateTime CreatedDate, IEnumerable<OrderItem> Items, Address ShippingAddress, OrderState State)
        {
            Id = id;
            this.Number = Number;
            this.CreatedDate = CreatedDate;
            this._Items = Items.ToList();
            this.ShippingAddress = ShippingAddress;
            this.State = State;
            OnInitialized();
        }

        partial void OnInitialized();
    }
}
```

> NOTE: For Aggregates and Entites, an ID class is generated in the same file.

> NOTE: Notice the `TotalPrice` is not initialized from the constructor. You are expected to initialize the property in the `OnInitialized()` based on other properties, the `Items` in this case.

A DB entity:
```
using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.DbEntity
{
    public class OrderDbEntity
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<OrderItem> Items { get; set; }

        public Address ShippingAddress { get; set; }

        public string State { get; set; }

    }
}
```

And a mapper:

```
using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Mapper
{
    public class OrderMapper : DbEntityMapper<Order, OrderDbEntity>
    {
        public OrderMapper(IDbEntityMapperFactory factory)
        	: base(factory)
        {
        }

        public override OrderDbEntity ToDbEntity(Order obj)
        {
            return new OrderDbEntity
            {
            	Number = obj.Number,
            	CreatedDate = obj.CreatedDate,
            	Items = MapDbEntityCollection(obj.Items),
            	ShippingAddress = MapChildDbEntity(obj.ShippingAddress),
            	State = MapDbEntityEnumeration(obj.State)
            };
        }

        public override Order ToDddObject(OrderDbEntity obj)
        {
            return new Order
            (
            	obj.Number,
            	obj.CreatedDate,
            	MapDddObjectCollection(obj.Items),
            	MapChildDddObject(obj.ShippingAddress),
            	MapDddObjectEnumeration(obj.State)
            );
        }
    }
}
```

#### Enumeration

And example for OrderState enumeration:
```
Kind: Enumeration
Name: OrderState
Properties:
  - Name: Name
    Type: string
Values:
  - Name: Created
    Properties: 
      Name: Created
  - Name: Canceled
    Properties: 
      Name: Canceled
```

Generates the following DDD oject class:
```
using System;
using System.Linq;
using System.Collections.Generic;
using HUGs.Generator.DDD.Framework.BaseModels;
using HUGs.Generator.DDD.Framework.Mapping;
using HUGs.DDD.Generated.Entity;
using HUGs.DDD.Generated.Aggregate;
using HUGs.DDD.Generated.ValueObject;
using HUGs.DDD.Generated.Enumeration;

namespace HUGs.DDD.Generated.Enumeration
{
    public class OrderState : HUGs.Generator.DDD.Framework.BaseModels.Enumeration
    {
        public static readonly OrderState Created = new(nameof(Created), "Created");

        public static readonly OrderState Canceled = new(nameof(Canceled), "Canceled");

        public string Name { get; }

        private OrderState(string internalName, string Name)
        	: base(internalName)
        {
            this.Name = Name;
        }

        public static OrderState FromString(string name)
        {
            return name switch
            {
            	"Created" => Created,
            	"Canceled" => Canceled,
            	_ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
```

#### Configuration
A configuration, that sets all of the namespaces and adds 2 additional usings to the generated files:

```
TargetNamespaces:
  ValueObject: My.Desired.Namespace.ValueObjects
  Entity: My.Desired.Namespace.Entities
  Aggregate: My.Desired.Namespace.Aggregates
  Enumeration: My.Desired.Namespace.Enumerations
  DbEntity: My.Desired.Namespace.DbEntities

AdditionalUsings:
  - My.Additional.Using1
  - My.Additional.Using2
```

Not all of the namespaces need to be set; for instance a configuration with custom namespace for only DB entities:

```
TargetNamespaces
  DbEntity: My.Desired.Namespace.DbEntities
```

