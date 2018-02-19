# AutoMock framework
This framework allows a cleaner way to mock dependencies on top of mocking frameworks. This allows developers to focus on the code being tested rather than on the mocking setup and its maintenance.
## Properties and Methods
###Target
An instance of the class being tested.
###The<T>
A generic method that returns the mocked dependency. If the dependency hasn't been set, it creates it.
## Examples
