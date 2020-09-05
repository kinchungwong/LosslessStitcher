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

Main article: [Readme_T3.md](Readme_T3.md)

