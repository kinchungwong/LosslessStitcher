### Three-Image Tracking ("T3 algorithm") (namespace: ```LosslessStitcher.Tracking.T3```)

#### Algorithm overview

The "T3" algorithm operates on three input images at a time. This algorithm finds overlapping content
that occurs on all three images, and computes a "trajectory" for each group of content. 

A trajectory refers to the movement vector between all three input images. If we denote the three images
as ```Image_0```, ```Image_1```, and ```Image_2```, the trajectory can be described as consisting of 
```Movement(Image_0, Image_1)``` and ```Movement(Image_1, Image_2)```. (The movement between ```Image_0``` 
and ```Image_2``` is easily calculated from the two, and therefore redundant.)

The main drawback of "T3" algorithm is that, when the image content is scrolled too quickly, there may 
be no overlapping image content that appears across three consecutive screenshots. For this reason, 
the "T2" and "T3" algorithms are never used alone; they are always used in conjunction, in order to cover
the corner cases that cannot be handled by either.

#### Algorithm steps

**Step 1.** Algorithm receives filtered unique hash points from three images.

**Step 2.** Algorithm analyzes unique hash points that occur on all three images.

**Step 3.** For each hash point triplet, the algorithm computes the relative movement across the three images.

**Step 4.** The algorithm builds a hash table based on the tuples of relative movement vectors. 

- Each tuple of relative movement vectors is known as a **trajectory**.
- Portions of the image content that move losslessly as a group are thus clustered together based
   on their trajectory.

**Step 5.** For each detected trajectory, the algorithm classifies the confidence likelihood (of the image 
content and its trajectory being relevant to a human user) and removes the low-confidence ones.

- The algorithm uses various spatial techniques. 
- To perform this filtering, the algorithm must re-run certain steps to recompute results. This causes
  changes to the integer index in the internal cross-reference tables. 
- The source code is structured to minimize programmer confusion throughout this recomputation process.

**Step 6.** The algorithm builds a spatial representation of clustered image content, grouped by trajectory 
and adjacency on the image, and also makes a best effort to find the precise boundary of such image content.

#### Remarks

If the algorithm determines that the image content has not moved between any two of the three images, the parent 
algorithm (the algorithm that uses the result of "T3") should either retry by replacing one of the duplicate image, 
or fall back to using Two-Image Tracking ("T2 algorithm").

