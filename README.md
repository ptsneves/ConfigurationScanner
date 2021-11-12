<a name='assembly'></a>
# ConfigurationScanner

## Contents

- [ConfigurationScanner](#T-ConfigurationScanner-ConfigurationScanner 'ConfigurationScanner.ConfigurationScanner')
  - [ClassSectionName](#F-ConfigurationScanner-ConfigurationScanner-ClassSectionName 'ConfigurationScanner.ConfigurationScanner.ClassSectionName')
  - [ConfigurationTokenKey](#F-ConfigurationScanner-ConfigurationScanner-ConfigurationTokenKey 'ConfigurationScanner.ConfigurationScanner.ConfigurationTokenKey')
  - [ThrowOnConfiguredForbiddenToken(conf)](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnConfiguredForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot- 'ConfigurationScanner.ConfigurationScanner.ThrowOnConfiguredForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot)')
  - [ThrowOnForbiddenToken(conf,forbiddenToken)](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot,System-String,System-Boolean- 'ConfigurationScanner.ConfigurationScanner.ThrowOnForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot,System.String,System.Boolean)')
- [ForbiddenValueConfigurationException](#T-ConfigurationScanner-ForbiddenValueConfigurationException 'ConfigurationScanner.ForbiddenValueConfigurationException')
  - [#ctor(foundValues)](#M-ConfigurationScanner-ForbiddenValueConfigurationException-#ctor-System-Collections-Generic-List{System-Collections-Generic-KeyValuePair{System-String,System-String}}- 'ConfigurationScanner.ForbiddenValueConfigurationException.#ctor(System.Collections.Generic.List{System.Collections.Generic.KeyValuePair{System.String,System.String}})')
  - [FoundValues](#P-ConfigurationScanner-ForbiddenValueConfigurationException-FoundValues 'ConfigurationScanner.ForbiddenValueConfigurationException.FoundValues')

<a name='T-ConfigurationScanner-ConfigurationScanner'></a>
## ConfigurationScanner `type`

##### Namespace

ConfigurationScanner

##### Summary

Extension class for IConfigurationRoot scanning.
See [ThrowOnForbiddenToken](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot,System-String,System-Boolean- 'ConfigurationScanner.ConfigurationScanner.ThrowOnForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot,System.String,System.Boolean)') and [ThrowOnConfiguredForbiddenToken](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnConfiguredForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot- 'ConfigurationScanner.ConfigurationScanner.ThrowOnConfiguredForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot)').
The goal of this library is to provide a simple way to ensure no "forbidden" values are found in the
configuration. The reason i personally use this library is that I have properties on the my appSettings which have
placeholder values that must be overriden by environmental variables, otherwise the program cannot run.
This library makes sure the forbidden values/placeholders are not in the final configuration.

<a name='F-ConfigurationScanner-ConfigurationScanner-ClassSectionName'></a>
### ClassSectionName `constants`

##### Summary

The hard coded name of the section used to look for the forbidden token. Can be read to programatically inject a forbidden token into
an existing configuration.
Value: "ConfigurationScanner".

<a name='F-ConfigurationScanner-ConfigurationScanner-ConfigurationTokenKey'></a>
### ConfigurationTokenKey `constants`

##### Summary

The hard coded name of the Configuration Token Key inside [ClassSectionName](#F-ConfigurationScanner-ConfigurationScanner-ClassSectionName 'ConfigurationScanner.ConfigurationScanner.ClassSectionName') section. Can be read to programatically inject
a forbidden token into an existing configuration.
Value: "Token".

<a name='M-ConfigurationScanner-ConfigurationScanner-ThrowOnConfiguredForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot-'></a>
### ThrowOnConfiguredForbiddenToken(conf) `method`

##### Summary

Has the same function as as [ThrowOnForbiddenToken](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot,System-String,System-Boolean- 'ConfigurationScanner.ConfigurationScanner.ThrowOnForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot,System.String,System.Boolean)') but the token is actually read from the configuration itself.
For this method to work there needs to be a section with the name equal to [ClassSectionName](#F-ConfigurationScanner-ConfigurationScanner-ClassSectionName 'ConfigurationScanner.ConfigurationScanner.ClassSectionName').
It also needs to have the key with the value equal to [ConfigurationTokenKey](#F-ConfigurationScanner-ConfigurationScanner-ConfigurationTokenKey 'ConfigurationScanner.ConfigurationScanner.ConfigurationTokenKey').
The actual value of the mentioned key contains the token that will be taken as forbidden.

##### Returns

The unmodified [IConfigurationRoot](#T-Microsoft-Extensions-Configuration-IConfigurationRoot 'Microsoft.Extensions.Configuration.IConfigurationRoot') if no forbidden token exists.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conf | [Microsoft.Extensions.Configuration.IConfigurationRoot](#T-Microsoft-Extensions-Configuration-IConfigurationRoot 'Microsoft.Extensions.Configuration.IConfigurationRoot') | The configuration root to be scanned. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | Thrown when the forbidden token cannot be found in the configuration root. |

<a name='M-ConfigurationScanner-ConfigurationScanner-ThrowOnForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot,System-String,System-Boolean-'></a>
### ThrowOnForbiddenToken(conf,forbiddenToken) `method`

##### Summary

Extension method that throws an exception when a given token is found on a value of the configuration root.
A configuration provider may have a property
of a secret set to a placeholder that another configuration provider is supposed to overwrite. If the overwrite
is not done this extension will throw an exception.
This is useful for example in the case where the appSettings json file has a structure where some properties
are secrets to be injected through other means like for example environmental variables. If the environmental
variables are not injected and thus not override the placeholders, the method will throw an exception and give
the program the opportunity to handle such situation.

##### Returns

The unmodified [IConfigurationRoot](#T-Microsoft-Extensions-Configuration-IConfigurationRoot 'Microsoft.Extensions.Configuration.IConfigurationRoot') if no forbidden token exists.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conf | [Microsoft.Extensions.Configuration.IConfigurationRoot](#T-Microsoft-Extensions-Configuration-IConfigurationRoot 'Microsoft.Extensions.Configuration.IConfigurationRoot') | The configuration root to be scanned. |
| forbiddenToken | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The token that, if found will throw an exception "T:ForbiddenValueConfigurationException". |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [ConfigurationScanner.ForbiddenValueConfigurationException](#T-ConfigurationScanner-ForbiddenValueConfigurationException 'ConfigurationScanner.ForbiddenValueConfigurationException') | Thrown when a forbidden token is found in the configuration. |

<a name='T-ConfigurationScanner-ForbiddenValueConfigurationException'></a>
## ForbiddenValueConfigurationException `type`

##### Namespace

ConfigurationScanner

##### Summary

Exception class that informs on when forbidden values were found on the configuration by
[ThrowOnForbiddenToken](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot,System-String,System-Boolean- 'ConfigurationScanner.ConfigurationScanner.ThrowOnForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot,System.String,System.Boolean)') or [ThrowOnConfiguredForbiddenToken](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnConfiguredForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot- 'ConfigurationScanner.ConfigurationScanner.ThrowOnConfiguredForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot)').

<a name='M-ConfigurationScanner-ForbiddenValueConfigurationException-#ctor-System-Collections-Generic-List{System-Collections-Generic-KeyValuePair{System-String,System-String}}-'></a>
### #ctor(foundValues) `constructor`

##### Summary

Initializes a new instance of the [ForbiddenValueConfigurationException](#T-ConfigurationScanner-ForbiddenValueConfigurationException 'ConfigurationScanner.ForbiddenValueConfigurationException') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| foundValues | [System.Collections.Generic.List{System.Collections.Generic.KeyValuePair{System.String,System.String}}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{System.Collections.Generic.KeyValuePair{System.String,System.String}}') | A list with the keys and values of the found forbidden properties found
in the configuration. |

<a name='P-ConfigurationScanner-ForbiddenValueConfigurationException-FoundValues'></a>
### FoundValues `property`

##### Summary

Gets list with the containing the configuration's key and value that were found and led
to the current exception.
