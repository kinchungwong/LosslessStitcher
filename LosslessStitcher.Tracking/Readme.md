### LosslessStitcher.Tracking

License type: MIT

#### Purpose

This module contains code for tracking the movement of lossless image content from one location on an image
to a possibly different location on one or more subsequent images.

#### How it works 

The input images are first processed with ```LosslessStitcher.Imaging.Hash2D```, where a 2D moving 
window hash code is computed at each pixel on each image.

The pixelwise hash code are further processed so that only an arbitrary subset of hash values will be used
for subsequent matching purpose. Also, only those hash values that occur exactly once on each input image
will be retained.

In other words, if the 2D moving window used for ```Hash2D``` computation has size ```(M x N)```, one can
conclude that the hash values that are retained after filtering are those that correspond to a unique 
configuration of ```(M x N)``` pixel values inside the corresponding window on the input image.

The identification and tracking of lossless image content movement becomes very easy.

However, a production-ready system needs to deal with the on-screen behaviors of many applications.
The handling of constant animations and image content movement requires much more work than the basic
concept of ```Hash2D```.

#### Code organization

The tracking algorithms are divided into two types: 

##### The two-image tracking ("T2") algorithm 

- The "T2" algorithm operates on two input images at a time. This algorithm finds the overlapping content
  between the two images. For each group of overlapping content, the "T2" algorithm will suggest a movement 
  vector.
  - However, when given real-world screenshot samples, the "T2" algorithm may find an overwhelming number
    of content clusters, each with a different movement vector. These may correspond to relevant image content 
    (of interest to a human user of the algorithm), unimportant movement (such as the scroll bar), or mere 
    collisions of ```Hash2D``` values.
  - To reliably reject unimportant and spurious content, the "T2" algorithm is used in conjunction with 
    another algorithm, which will be explained next.

##### The three-image tracking ("T3") algorithm

- The "T3" algorithm operates on three input images at a time. This algorithm finds overlapping content
  that occurs on all three images, and computes a "trajectory" for each group of content. 
- A trajectory refers to the movement vector between all three input images. If we denote the three images
  as ```Image_0```, ```Image_1```, and ```Image_2```, the trajectory can be described as consisting of 
  ```Movement(Image_0, Image_1)``` and ```Movement(Image_1, Image_2)```. (The movement between ```Image_0``` 
  and ```Image_2``` is easily calculated from the two, and therefore redundant.)
- The main drawback of "T3" algorithm is that, when the image content is scrolled too quickly, there may 
  be no overlapping image content that appears across three consecutive screenshots. For this reason, 
  the "T2" and "T3" algorithms are never used alone; they are always used in conjunction, in order to cover
  the corner cases that cannot be handled by either.
