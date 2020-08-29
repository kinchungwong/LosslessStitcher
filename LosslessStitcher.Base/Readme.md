### LosslessStitcher.Base

This project contains lightweight data types that are used concretely in defining interfaces. 
As such, these data types are not themselves injectable because using them via an interface
would have defeated the purpose of having these data types.

The concrete lightweight data types in this project uses HashCodeUtility to implement their
```GetHashCode``` methods.

License type: MIT
