namespace DeckOfPlayingCardsTests
{
    public class Tests
    {
        /// <summary>
        /// Tests whether the deck shuffle is actually random.
        /// So: the distribution of cards is uniform.
        /// </summary>
        [Test]
        public void ShuffleIsRandom()
        {
            // Initiate some variables.
            var random = new Random(1);
            var deck = Deck.Get();
            var numberOfIterations = 1000;

            // Dictionary to store the frequency of each card after shuffling.
            var cardFrequency = new Dictionary<Card, int>();

            foreach (var card in deck.Cards)
            {
                cardFrequency[card] = 0;
            }

            // Perform shuffling multiple times and count the frequency of each card.
            for (var i = 0; i < numberOfIterations; i++)
            {
                // Include the random variable so that the default doesn't take the same seed.
                deck.Shuffle(random: random);

                // Store which index the each card is shuffled.
                var cardIndex = 0;

                foreach (var card in deck.Cards)
                {
                    cardFrequency[card] = cardFrequency[card] + cardIndex;
                    cardIndex++;
                }
            }

            // Calculate the expected average card index after shuffling (assuming a uniform distribution).
            var averageIndex = (52 - 1) / 2.0;

            // Calculate the chi-square statistic
            double chiSquare = 0;

            foreach (var card in deck.Cards)
            {
                // The observer average index is the observed indexes summed up divided by the number of shuffles.
                var observedAverageIndex = (double)cardFrequency[card] / numberOfIterations;

                // Calculate the chi-square contribution for this card.
                var chiSquareContribution =
                    Math.Pow(observedAverageIndex - averageIndex, 2) / averageIndex;
                chiSquare += chiSquareContribution;
            }

            // Degrees of freedom for a deck of cards is (number of cards - 1).
            var degreesOfFreedom = 52 - 1;

            // Set the significance level (alpha) for the chi-square test.
            // 0.05 here means we want to be 95% certain the distibution is uniform.
            var alpha = 0.05;

            // Use chi-square distribution table to get the critical value for the given significance level and degrees of freedom.
            var chiSquaredDist = new ChiSquared(degreesOfFreedom);
            var criticalValue = chiSquaredDist.InverseCumulativeDistribution(1 - alpha);

            // Perform the chi-square test.
            Assert.That(chiSquare, Is.LessThan(criticalValue), "The deck shuffle is not random.");
        }

        /// <summary>
        /// Tests whether the cards that are already drawn, don't get shuffled back into the deck if so specified.
        /// </summary>
        [Test]
        public void DrawnCardsDoNotShuffle()
        {
            var deck = Deck.Get();
            deck.Shuffle();
            var numberOfCards = 10;

            var firstTenCards = new List<Card>();

            // Draw 10 cards.
            for (var i = 0; i < numberOfCards; i++)
            {
                firstTenCards.Add(deck.Draw()!);
            }

            // Reset the deck so the already drawn cards go back onto the top.
            deck.Reset();

            for (var i = 0; i < numberOfCards / 2; i++)
            {
                _ = deck.Draw();
            }

            // Shuffle the remaining cards in the deck.
            deck.Shuffle(includeDrawnCards: false);

            // Reset the deck so the already drawn cards go back onto the top.
            deck.Reset();

            // The first five cards should be the same.
            for (var i = 0; i < numberOfCards / 2; i++)
            {
                Assert.That(
                    firstTenCards[i],
                    Is.EqualTo(deck.Draw()),
                    $"One of the first {numberOfCards / 2} cards is not the same"
                );
            }

            var numberOfCardsDifferent = 0;

            // The second five cards should not be the same.
            for (var i = numberOfCards / 2; i < numberOfCards; i++)
            {
                if (!firstTenCards[i].Equals(deck.Draw()!))
                {
                    numberOfCardsDifferent++;
                }
            }

            var threshold = 4;

            Assert.That(
                numberOfCardsDifferent,
                Is.GreaterThanOrEqualTo(threshold),
                $"Less than {threshold} cards are different."
            );
        }

        /// <summary>
        /// Tests whether every card has a display string and that the methods called do not throw an exception.
        /// </summary>
        [Test]
        public void DisplayEveryCard()
        {
            var deck = Deck.Get();

            foreach (var card in deck.Cards)
            {
                var assertionMessage =
                    $"Exception thrown while getting the display string of the {card.Rank} of {card.Suit}";

                Assert.Multiple(() =>
                {
                    Assert.That(
                        () =>
                        {
                            _ = card.GetDisplayString(displayTenAsT: false);
                        },
                        Throws.Nothing,
                        assertionMessage
                    );
                    Assert.That(
                        () =>
                        {
                            _ = card.GetDisplayString(displayTenAsT: true);
                        },
                        Throws.Nothing,
                        assertionMessage
                    );
                });
            }
        }

        /// <summary>
        /// Test whether creating an empty deck has valid results.
        /// </summary>
        [Test]
        public void EmptyDeckIsValid()
        {
            var deck = Deck.Get(new List<Card>());

            Assert.Multiple(() =>
            {
                Assert.That(() => deck.Shuffle(), Throws.Nothing, "Shuffle throws exception");
                Assert.That(
                    () =>
                    {
                        _ = deck.Draw();
                    },
                    Throws.Nothing,
                    "Draw throws exception"
                );
                Assert.That(() => deck.Reset(), Throws.Nothing, "Reset throws exception");
                Assert.That(deck.Draw(), Is.EqualTo(null), "Drawn card is not null");
            });
        }

        /// <summary>
        /// Test whether creating a deck with only one card has valid results.
        /// </summary>
        [Test]
        public void DeckWithOneCardIsValid()
        {
            var cards = new List<Card>() { new(Rank.Ace, Suit.Spades) };
            var deck = Deck.Get(cards);

            Assert.Multiple(() =>
            {
                Assert.That(() => deck.Shuffle(), Throws.Nothing, "Shuffle throws exception");
                Assert.That(
                    () =>
                    {
                        _ = deck.Draw();
                    },
                    Throws.Nothing,
                    "Draw throws exception"
                );
                Assert.That(() => deck.Reset(), Throws.Nothing, "Reset throws exception");
                Assert.That(
                    deck.Draw(),
                    Is.EqualTo(cards.First()),
                    "Drawn card is not the card in the deck"
                );
            });
        }
    }
}
