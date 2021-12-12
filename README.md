# HUGs
### _**H**elpful and **U**seful **G**enerators of **S**ource_
An opinionated .NET Source Generator to help with the DDD boilerplate code.

## Description
**HUGs** is a** Source Generator** framework that targets **DDD object classes generation**. The Source Generator uses `Additional Files` with DDD object schemas in YAML format to build the DDD model and generate the DDD object classes for **ValueObjects**, **Entities**, **Aggregates**, and **Enumerations**. Moreover, **HUGs** also generates a plain **DbEntity** class that is database-friendly, and it generates a **Mapper** that can map a DDD object to a database object and vice versa. Therefore, **HUGs** can generate up to 3 classes from a single YAML schema.

## Installation
TODO

## Usage

**HUGs** require YAML files with file extension `.dddschema` and `.dddconfig` for 

Schemas for **Entity**, **Aggregate**, and **ValueObject** DDD object expect the follow structure of the corresponding `Additional File`:

```
Kind: ValueObject | Entity | Aggregate 
Name: <NameOfTheClassHere>
Properties:
  - Name: <NameOfProperty>
    Type: <TypeOfProperty>
    Computed: true | false
```

The `Kind` specifies the type of DDD object. The `Kind` affects the structure of the generated code and the inherited class. The generated class name is specified by the `Name`, and the class properties can be specified as a list in `Properties`. The property requires its `Name` and `Type`. The `Type` must be a C\# primitive type or another DDD object reference. The type also supports nullability `?` and `Computed` properties. The computed properties is not initialized in a constructor.

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

Only **ONE** configuration file is expected!

### Examples

TODO
