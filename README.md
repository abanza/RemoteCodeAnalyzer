# Remote Code Maintainability Analyzer
This sample project develops a program for quality analysis of C# code to measure the maintainability of functions and classes. 
Different code metrics are analyzed to generate a final maintainability index. The Code Maintainability Analyzer computes 
the following metrics: size (LOC) and cyclomatic complexity for each function, and cohesion and coupling for each class in each
file of a specified file set.

- Size: The tool computes size in terms of the number of lines of code.

- Cyclomatic Complexity: This refers to the number of independent paths in a function.

- Cohesion: We are going to use one of the simplest structural cohesion metrics proposed by Chidamber & Kemerer in 1991 known as 
lack of cohesion in methods (LCOM1). LCOM1 measures the cohesion in terms of the number of pairs of functions that do not 
share any data members. To learn more, check here http://www.cs.uah.edu/~letzkorn/joop.pdf

- Coupling: we will use this metric to measure dependency between classes. A class A depends on class B if A inherits, aggregates,
composes, or uses class B. We are going to use the number of classes that one class depends on as the coupling measure for that class.

Finally, the Code Maintainability Analyzer computes a maintainability index for each class using these four metrics as:
MI=a * Size + b * Cyclomatic Complexity + c * Cohesion + d * Coupling
The purpose of this code analysis is to find functions and classes that may need attention because of their quality metrics. 
A secondary purpose is to help potential developers or managers quickly understand the structure of a set of code.
