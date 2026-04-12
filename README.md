# Deck of playing cards

[![Nuget](https://img.shields.io/nuget/v/DeckOfPlayingCards?color=green)](https://www.nuget.org/packages/DeckOfPlayingCards)

## Table of Contents

- [About](#about)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing and Feedback](#contributing-and-feedback)

## About

With this library, you can create a standard deck of 52 cards, shuffle it, and draw cards from it. You can also reset the deck if you want to run it again. This library can be used to create popular card games such as poker or blackjack. You can also create custom decks using the default set of cards, allowing you to play multi-deck blackjack or "oops all jacks" poker.

## Features

This library includes the following classes:
* Entities:
  * Card.cs
  * Deck.cs
* Enums:
  * CardRank.cs
  * CardSuit.cs

The Card.cs class is a simple class for storing the rank and the suit of the card.
The Deck.cs class contains a list of cards as a property. It also includes the following methods:
* Get()
	* Gets a standard deck of 52 cards, not shuffled.
	* If specified, you can create a deck with your own set of cards.
* Shuffle()
	* Shuffles the deck.
	* Includes a parameter to specify whether the already drawn cards should be shuffled back into the deck.
	* A Random variable can be passed in. If not, a Random variable is initialized.
* Draw()
	* Draws a card from the deck.
	* Returns the drawn card
* Reset()
	* Resets the deck, meaning all drawn cards will be put back onto the deck in the same order as they were originally.

The Draw() method doesn't remove a card from the deck, it simply returns the card at the current index and increments the index by 1. This is how already drawn cards can be put back into the deck in the same order.

For the source code, check out the GitHub repo.

## Installation

To use Deck of Playing Cards in your project, you can install it via NuGet Package Manager:

```shell
nuget install DeckOfPlayingCards
```

## Usage

Getting a deck, shuffling it, drawing cards, and resetting it is pretty straightforward. See the code snippet below.

```cs
using DeckOfCardsLibrary;

// Get a standard unshuffled deck.
var deck = Deck.get();

// Shuffle the deck.
deck.shuffle();

// Draw some cards.
var firstCard = deck.draw()!.Value;
var secondCard = deck.draw()!.Value;

// Keep in mind that deck.draw() returns null if all the cards in the deck have already been drawn.
// In this case I have only drawn two cards, so I can safely say the method does not return null.

// Let's see what cards we got!
Console.WriteLine(firstCard);
Console.WriteLine(secondCard);

// That was a good hand! Let's reset the deck so I get the same cards again.
deck.reset();

// Just kidding! I play fair.
deck = Deck.get();
deck.shuffle(random: new Random(103413));
```

The GitHub repo also includes a console app which you can play around with.

## Contributing and Feedback

If you have questions, concerns, or would like to contribute to this project, there are several ways to get involved:

- **Open an Issue**: If you encounter a bug, have a feature request, or want to discuss improvements, please [open an issue](https://github.com/LarsGast/DeckOfPlayingCards/issues).

- **Start a Discussion**: You can also initiate a discussion in the [Discussions](https://github.com/LarsGast/DeckOfPlayingCards/discussions) section of this repository to share ideas or seek help.

- **Fork and Contribute**: If you'd like to make changes or enhancements to the project, fork the repository and submit a pull request with your proposed changes.

Your contributions and feedback are highly valued and essential to the improvement of this project. Thank you for your support!
