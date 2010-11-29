One note about the signing of this library. I (Lasse V. Karlsen) will be releasing
binary releases of it, signed with a key I will hold private. 

To build these binaries I will be using the special build configuration named
`ReleaseBinaries`, which will reference the public key of my own private key,
and will thus not build on your machine.

However, both `Debug` and `Release` builds will work fine, but note that any
binaries you build on your own machine will be signature-incompatible with any
official binaries I release (when I release them), and so any projects in which
you use these libraries you will have to replace the references if you switch
from a private build to an official one, or vice versa.