DiffLib
===

[![build](https://github.com/lassevk/DiffLib/actions/workflows/build.yml/badge.svg)](https://github.com/lassevk/DiffLib/actions/workflows/build.yml)
[![codecov](https://codecov.io/gh/lassevk/DiffLib/graph/badge.svg?token=N58US136E7)](https://codecov.io/gh/lassevk/DiffLib)
[![codeql](https://github.com/lassevk/DiffLib/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/lassevk/DiffLib/actions/workflows/github-code-scanning/codeql)

This project contains code that will do Diff generation, comparing
two collections of any type and outputting the differences between
the two, including an attempt to align up differences, specifically
in text files.

For now, the merge functionality has been removed. This might be added back in
the future, but there are serious issues with the merge functionality that I
am unable to resolve. Likely the entire merge functionality has to be reworked
from the ground. So the new .NET 8/9 versions of the library will not contain
the merge functionality. If I can find a way to reimplement the functionality,
or fix the issues with the current implementation, I will add them back,
but right now the demand for a .NET 8 and 9 compatible version is higher
than the number of people having used the merge functions.

<strike>The library also comes with 3-way merge functionality, allowing you
to take two collections that both started as a 3rd common base but
has been modified, and then merge the modifications into a final
output. This is the type of 3-way merge that version control systems
do.</strike>

Details:

* Language: C# 13.0
* Runtimes:
  * .NET 8
  * .NET 9

*(note, there are older versions of the package that supports older versions of .NET)*

Repository and project location: [GitHub][1]  
Maintainer: [Lasse V. Karlsen][2]

---

  [1]: https://github.com/lassevk/DiffLib
  [2]: mailto:lasse@vkarlsen.no
