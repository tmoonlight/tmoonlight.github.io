---
title: Thedifferencecharthepointerandchar[]thearrayishowyouinteractwitht
date: 2014/8/19 7:08:40
tags:
---


The difference char* the pointer and char[] the array is how you interact with them after you create them.

If you are just printing the two examples will perform exactly the same. They both generate data in memory, {h,e,l,l,o,/0}.

The fundamental difference is that in one char* you are assigning it to a pointer, which is a variable. In char[] you are assigning it to an array which is not a variable.

char[] is a structure, it is specific section of memory, it allows for things like indexing, but it always will start at the address that currently hold's 'h'.

char* is a variable. It was initialize with a number, but we can change number using mathematical operators such as ++, because it is essentially an integer.

So here's one example, where the pointer would be much more efficient than an array. Say for whatever reason we wanted the string to say "ello" instead of "hello". With a pointer all we need to do is shift the pointer one to the "right"

char* p = "hello"; p++;

This is a very fast operation and runs in Big O of 1 (literally in this case, i is one very fast operation)

But with char[], we can't change where the array starts, we actually therefore need to do something much less efficient, we need to loop through the entire word and for every index change the char in memory. It would look something like this. (manually done instead of looping for clarity)

char s[] = "hello"; s[0] = 'e' s[1] = 'l' s[2] = 'l' s[3] = 'o' s[4] = '/0'

This is a far slower operation running in "big O of n".
