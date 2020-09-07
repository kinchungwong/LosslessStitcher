### ```CollectionsUtility``` (Part of ```LosslessStitcher```)

#### UniqueList

- It is an append-only list of items where each item can only occur once.
- This list retains the order in which items are added.
- When each item is added for the first time, it is assigned an index, much like the item index on a ```List```.
- If a duplicate item is added, nothing is changed.
- Internally, a ```Dictionary``` (and ```EqualityComparer.Default```) is used for equality comparison.

- ```UniqueList{T}``` implements ```IList{T}``` but with the following differences:
  - 