This project contains code that will do Diff generation, comparing
two collections of any type and outputting the differences between
the two, including an attempt to align up differences, specifically
in text files.

The library also comes with 3-way merge functionality, allowing you
to take two collections that both started as a 3rd common base but
has been modified, and then merge the modifications into a final
output. This is the type of 3-way merge that version control systems
do.

Details:

* Language: C# 12.0
* Runtimes:
  * .NET Standard 1.0
  * .NET Standard 2.0
  * .NET Standard 2.1
  * .NET 5
  * .NET 6
  * .NET 7
  * .NET 8

Repository and project location: [GitHub][1]  
Maintainer: [Lasse V. Karlsen][2]

---

  [1]: https://github.com/lassevk/DiffLib
  [2]: mailto:lasse@vkarlsen.no
