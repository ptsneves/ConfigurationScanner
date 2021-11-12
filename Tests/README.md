<a name='assembly'></a>
# Tests

## Contents

- [UnitTest](#T-Tests-UnitTest 'Tests.UnitTest')
  - [TestNoTokenDoesNotThrow()](#M-Tests-UnitTest-TestNoTokenDoesNotThrow 'Tests.UnitTest.TestNoTokenDoesNotThrow')
  - [TestNoTokenWithConfDoesNotThrow()](#M-Tests-UnitTest-TestNoTokenWithConfDoesNotThrow 'Tests.UnitTest.TestNoTokenWithConfDoesNotThrow')
  - [TestReturnsIdentity()](#M-Tests-UnitTest-TestReturnsIdentity 'Tests.UnitTest.TestReturnsIdentity')
  - [TestThrowOnConfiguredForbiddenTokenWithNoConfThrows()](#M-Tests-UnitTest-TestThrowOnConfiguredForbiddenTokenWithNoConfThrows 'Tests.UnitTest.TestThrowOnConfiguredForbiddenTokenWithNoConfThrows')
  - [TestThrowOnConfiguredForbiddenTokenWorks()](#M-Tests-UnitTest-TestThrowOnConfiguredForbiddenTokenWorks 'Tests.UnitTest.TestThrowOnConfiguredForbiddenTokenWorks')
  - [TestWithConfThrowsWhenNotIgnoreConf()](#M-Tests-UnitTest-TestWithConfThrowsWhenNotIgnoreConf 'Tests.UnitTest.TestWithConfThrowsWhenNotIgnoreConf')

<a name='T-Tests-UnitTest'></a>
## UnitTest `type`

##### Namespace

Tests

##### Summary

Unit tests for [ConfigurationScanner](#T-ConfigurationScanner-ConfigurationScanner 'ConfigurationScanner.ConfigurationScanner').

<a name='M-Tests-UnitTest-TestNoTokenDoesNotThrow'></a>
### TestNoTokenDoesNotThrow() `method`

##### Summary

We want to make sure that [ThrowOnForbiddenToken](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot,System-String,System-Boolean- 'ConfigurationScanner.ConfigurationScanner.ThrowOnForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot,System.String,System.Boolean)')> does not throw when
no forbidden token is found.

##### Parameters

This method has no parameters.

<a name='M-Tests-UnitTest-TestNoTokenWithConfDoesNotThrow'></a>
### TestNoTokenWithConfDoesNotThrow() `method`

##### Summary

Test that a configuration defining the forbidden token is not itself caught as a violation.

##### Parameters

This method has no parameters.

<a name='M-Tests-UnitTest-TestReturnsIdentity'></a>
### TestReturnsIdentity() `method`

##### Summary

This tests whether the scan does not when no forbidden values are in the configuration.
It also check that the return value is the same as the original [IConfigurationRoot](#T-Microsoft-Extensions-Configuration-IConfigurationRoot 'Microsoft.Extensions.Configuration.IConfigurationRoot')
passed to us.

##### Parameters

This method has no parameters.

<a name='M-Tests-UnitTest-TestThrowOnConfiguredForbiddenTokenWithNoConfThrows'></a>
### TestThrowOnConfiguredForbiddenTokenWithNoConfThrows() `method`

##### Summary

Test that we throw a [ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') exception when we expect a configured forbidden
token but no such configuration can be found.

##### Parameters

This method has no parameters.

<a name='M-Tests-UnitTest-TestThrowOnConfiguredForbiddenTokenWorks'></a>
### TestThrowOnConfiguredForbiddenTokenWorks() `method`

##### Summary

This is the most complex test which takes the forbidden token from the ConfigurationRoot itself.
This test uses a test-configuration.json configuration file that has a more complex structure as well.
This test also illustrates the usage of the const fields [ClassSectionName](#F-ConfigurationScanner-ConfigurationScanner-ClassSectionName 'ConfigurationScanner.ConfigurationScanner.ClassSectionName') and
[ConfigurationTokenKey](#F-ConfigurationScanner-ConfigurationScanner-ConfigurationTokenKey 'ConfigurationScanner.ConfigurationScanner.ConfigurationTokenKey') to inject configuration for
[ThrowOnConfiguredForbiddenToken](#M-ConfigurationScanner-ConfigurationScanner-ThrowOnConfiguredForbiddenToken-Microsoft-Extensions-Configuration-IConfigurationRoot- 'ConfigurationScanner.ConfigurationScanner.ThrowOnConfiguredForbiddenToken(Microsoft.Extensions.Configuration.IConfigurationRoot)'), even though this is not really necessary for our test, it
serves as an illustration that you can do it.

##### Parameters

This method has no parameters.

<a name='M-Tests-UnitTest-TestWithConfThrowsWhenNotIgnoreConf'></a>
### TestWithConfThrowsWhenNotIgnoreConf() `method`

##### Summary

Test that if we do not ignore our own configuration section we will detect it as a fault.

##### Parameters

This method has no parameters.
