﻿using System;
using System.Collections.Generic;
using UsingDirectiveForAdditionalTypes;


Console.WriteLine("*");
Console.WriteLine(nameof(UsingExamples));
var articles = UsingExamples.GetArticles();
foreach (var article in articles)
{
    Console.WriteLine($"{article.Title}:{article.WordsCount}");
}

int minInt = UsingExamples.GetMinimumInteger(5, 3, 4, 6);
Console.WriteLine($"The minimum int is {minInt}");


Console.WriteLine("*");
Console.WriteLine(nameof(AliasExamples));
var codeMazeArticles = AliasExamples.GetArticles();
foreach (var codeMazeArticle in codeMazeArticles)
{
    Console.WriteLine($"{codeMazeArticle.Title}:{codeMazeArticle.WordsCount}");
}


Console.WriteLine("*");
Console.WriteLine(nameof(AdditionalTypesExamples));
var ints = AdditionalTypesExamples.GetInts();
foreach (var number in ints)
{
    Console.WriteLine($"The number is: {number?.ToString() ?? "null"}");
}

var articleTitles = AdditionalTypesExamples.GetArticleTitles();
foreach (var articleTitle in articleTitles)
{
    Console.WriteLine(articleTitle);
}

var points = AdditionalTypesExamples.GetPointsOfInterest();
foreach (var point in points)
{
    Console.WriteLine($"X:{point.X}, Y:{point.Y}");
}

var (Location, Distance) = new Destination("London", 2500.00);
Console.WriteLine($"My destination is {Location} with distance {Distance}");
Console.WriteLine($"My destination is {destination.Location} with distance {destination.Distance}");