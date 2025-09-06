using System;
using System.Collections.Generic;
using System.Linq;

enum MatchCondition
{
    FaceValue,
    Suit,
    Both
}

enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

enum Face
{
    Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
    Jack, Queen, King, Ace
}

record Card(Face Face, Suit Suit)
{
    public override string ToString() => $"{Face} of {Suit}";
}

class Player
{
    public string Name { get; }
    public List<Card> WonCards { get; } = new();

    public Player(string name) => Name = name;
}

class SnapGame
{
    private readonly int _numPacks;
    private readonly MatchCondition _condition;
    private readonly List<Card> _pile = new();
    private readonly Player[] _players = { new("Player 1"), new("Player 2") };
    private readonly Random _rng = new();

    public SnapGame(int numPacks, MatchCondition condition)
    {
        _numPacks = numPacks;
        _condition = condition;
        BuildAndShufflePile();
    }

    private void BuildAndShufflePile()
    {
        var tempPile = new List<Card>();
        for (int p = 0; p < _numPacks; p++)
        {
            foreach (Suit suit in Enum.GetValues<Suit>())
                foreach (Face face in Enum.GetValues<Face>())
                    tempPile.Add(new Card(face, suit));
        }
        _pile.Clear();
        _pile.AddRange(tempPile.OrderBy(_ => _rng.Next()));
    }

    private bool IsMatch(Card a, Card b)
    {
        return _condition switch
        {
            MatchCondition.FaceValue => a.Face == b.Face,
            MatchCondition.Suit => a.Suit == b.Suit,
            MatchCondition.Both => a.Face == b.Face && a.Suit == b.Suit,
            _ => false
        };
    }

    public void Play()
    {
        List<Card> table = new();
        Card? prev = null;

        while (_pile.Count > 0)
        {
            var card = _pile[0];
            _pile.RemoveAt(0);
            table.Add(card);

            if (prev != null && IsMatch(prev, card))
            {
                int winnerIdx = _rng.Next(2);
                var winner = _players[winnerIdx];
                winner.WonCards.AddRange(table);
                Console.WriteLine($"SNAP! {winner.Name} wins {table.Count} cards: {string.Join(", ", table)}");
                table.Clear();
            }
            prev = card;
        }

        // Discard any remaining cards on the table
        if (table.Count > 0)
            Console.WriteLine($"No snap: {table.Count} cards discarded.");

        // Results
        Console.WriteLine();
        foreach (var player in _players)
            Console.WriteLine($"{player.Name} won {player.WonCards.Count} cards.");

        if (_players[0].WonCards.Count > _players[1].WonCards.Count)
            Console.WriteLine($"{_players[0].Name} wins!");
        else if (_players[1].WonCards.Count > _players[0].WonCards.Count)
            Console.WriteLine($"{_players[1].Name} wins!");
        else
            Console.WriteLine("It's a draw!");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Snap! Simulation");

        int packs;
        while (true)
        {
            Console.Write("Enter number of packs (1 or more): ");
            if (int.TryParse(Console.ReadLine(), out packs) && packs > 0)
                break;
            Console.WriteLine("Invalid input.");
        }

        Console.WriteLine("Select matching condition:");
        Console.WriteLine("1. Face value");
        Console.WriteLine("2. Suit");
        Console.WriteLine("3. Both face value and suit");
        MatchCondition condition = MatchCondition.FaceValue;
        while (true)
        {
            Console.Write("Enter 1, 2, or 3: ");
            var input = Console.ReadLine();
            if (input == "1") { condition = MatchCondition.FaceValue; break; }
            if (input == "2") { condition = MatchCondition.Suit; break; }
            if (input == "3") { condition = MatchCondition.Both; break; }
            Console.WriteLine("Invalid input.");
        }

        var game = new SnapGame(packs, condition);
        game.Play();
    }
}
