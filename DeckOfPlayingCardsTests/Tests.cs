namespace DeckOfCardsTests
{
    public class Tests
    {
        /// <summary>
        /// Tests whether the deck shuffle is actually random.
        /// So: the distribution of cards is uniform.
        /// </summary>
        [Test]
        public void shuffleIsRandom()
        {
            // Initiate some variables.
            var random = new Random(1);
            var deck = Deck.get();
            int numberOfIterations = 1000;

            // Dictionary to store the frequency of each card after shuffling.
            var cardFrequency = new Dictionary<Card, int>();

            foreach (var card in deck.cards)
            {
                cardFrequency[card] = 0;
            }

            // Perform shuffling multiple times and count the frequency of each card.
            for (int i = 0; i < numberOfIterations; i++)
            {
                // Include the random variable so that the default doesn't take the same seed.
                deck.shuffle(random: random);

                // Store which index the each card is shuffled.
                int cardIndex = 0;

                foreach (var card in deck.cards)
                {
                    cardFrequency[card] = cardFrequency[card] + cardIndex;
                    cardIndex++;
                }
            }

            // Calculate the expected average card index after shuffling (assuming a uniform distribution).
            double averageIndex = (52 - 1) / 2.0;

            // Calculate the chi-square statistic
            double chiSquare = 0;

            foreach (var card in deck.cards)
            {
                // The observer average index is the observed indexes summed up divided by the number of shuffles.
                double observedAverageIndex = (double)cardFrequency[card] / numberOfIterations;

                // Calculate the chi-square contribution for this card.
                double chiSquareContribution =
                    Math.Pow(observedAverageIndex - averageIndex, 2) / averageIndex;
                chiSquare += chiSquareContribution;
            }

            // Degrees of freedom for a deck of cards is (number of cards - 1).
            int degreesOfFreedom = 52 - 1;

            // Set the significance level (alpha) for the chi-square test.
            // 0.05 here means we want to be 95% certain the distibution is uniform.
            double alpha = 0.05;

            // Use chi-square distribution table to get the critical value for the given significance level and degrees of freedom.
            ChiSquared chiSquaredDist = new ChiSquared(degreesOfFreedom);
            double criticalValue = chiSquaredDist.InverseCumulativeDistribution(1 - alpha);

            // Perform the chi-square test.
            Assert.That(chiSquare, Is.LessThan(criticalValue), "The deck shuffle is not random.");
        }

        /// <summary>
        /// Tests whether the cards that are already drawn, don't get shuffled back into the deck if so specified.
        /// </summary>
        [Test]
        public void drawnCardsDoNotShuffle()
        {
            var deck = Deck.get();
            deck.shuffle();
            var numberOfCards = 10;

            var firstTenCards = new List<Card>();

            // Draw 10 cards.
            for (int i = 0; i < numberOfCards; i++)
            {
                firstTenCards.Add(deck.draw());
            }

            // Reset the deck so the already drawn cards go back onto the top.
            deck.reset();

            for (int i = 0; i < numberOfCards / 2; i++)
            {
                deck.draw();
            }

            // Shuffle the remaining cards in the deck.
            deck.shuffle(includeDrawnCards: false);

            // Reset the deck so the already drawn cards go back onto the top.
            deck.reset();

            // The first five cards should be the same.
            for (int i = 0; i < numberOfCards / 2; i++)
            {
                Assert.That(
                    firstTenCards[i].equals(deck.draw()),
                    Is.True,
                    $"One of the first {numberOfCards / 2} cards is not the same"
                );
            }

            var numberOfCardsDifferent = 0;

            // The second five cards should not be the same.
            for (int i = numberOfCards / 2; i < numberOfCards; i++)
            {
                if (!firstTenCards[i].equals(deck.draw()))
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
        public void displayEveryCard()
        {
            var deck = Deck.get();

            foreach (var card in deck.cards)
            {
                var assertionMessage =
                    $"Exception thrown while getting the display string of the {card.rank} of {card.suit}";

                Assert.Multiple(() =>
                {
                    Assert.That(
                        () =>
                        {
                            card.getDisplayString(displayTenAsT: false);
                        },
                        Throws.Nothing,
                        assertionMessage
                    );
                    Assert.That(
                        () =>
                        {
                            card.getDisplayString(displayTenAsT: true);
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
        public void emptyDeckIsValid()
        {
            var deck = Deck.get(new List<Card>());

            Assert.Multiple(() =>
            {
                Assert.That(
                    () =>
                    {
                        deck.shuffle();
                    },
                    Throws.Nothing,
                    "Shuffle throws exception"
                );
                Assert.That(
                    () =>
                    {
                        deck.draw();
                    },
                    Throws.Nothing,
                    "Draw throws exception"
                );
                Assert.That(
                    () =>
                    {
                        deck.reset();
                    },
                    Throws.Nothing,
                    "Reset throws exception"
                );
                Assert.That(deck.draw(), Is.EqualTo(null), "Drawn card is not null");
            });
        }

        /// <summary>
        /// Test whether creating a deck with only one card has valid results.
        /// </summary>
        [Test]
        public void deckWithOneCardIsValid()
        {
            var cards = new List<Card>() { new Card(Card.Rank.Ace, Card.Suit.Spades) };
            var deck = Deck.get(cards);

            Assert.Multiple(() =>
            {
                Assert.That(
                    () =>
                    {
                        deck.shuffle();
                    },
                    Throws.Nothing,
                    "Shuffle throws exception"
                );
                Assert.That(
                    () =>
                    {
                        deck.draw();
                    },
                    Throws.Nothing,
                    "Draw throws exception"
                );
                Assert.That(
                    () =>
                    {
                        deck.reset();
                    },
                    Throws.Nothing,
                    "Reset throws exception"
                );
                Assert.That(
                    deck.draw(),
                    Is.EqualTo(cards.First()),
                    "Drawn card is not the card in the deck"
                );
            });
        }
    }
}
