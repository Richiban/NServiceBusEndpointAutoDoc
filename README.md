# NServiceBusEndpointAutoDoc

## Introduction

This project is an executable that will scan a given assembly (and any referenced assemblies that it needs to) for XML doc comments
in order to automatically build Markdown documentation of the NServiceBus handlers in the project as well as the message types that they handle.

## Example output:

# MyCompany.MyNamespace.MyMessageType

Any comments that are on my handler implementation class go here

Any comments on the specific Handle method for this message type go here

Any comments on the message type itself go here. Below are the names, types, and any comments on the properties of the message type:


| Field           | Type           | Comment                               |
| --------------- | -------------- | ------------------------------------- |
| SomeProperty    | Guid           | This is my comment about SomeProperty |
| AnotherProperty | DateTimeOffset | AnotherProperty's comments go here    |