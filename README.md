# Chess on C#

## Architecture
- Chess.Core holds the game logic (pieces, board, check/pin state, clock, move history)
- Chess.Ui is an Avalonia MVVM front end
- Chess.Tests is xUnit v3.
- Move legality is computed through CheckState (attackers, blocking squares) and pin tracking (DetectAndMarkPin), rather than by simulating each move and testing for check.

## Screenshots
### Time selection
<img src="Screenshots/chess-1.png" alt="Chess window with time selection">

### On game
<img src="Screenshots/chess-2.png" alt="Chess window on game with clock">

### Available moves
<img src="Screenshots/chess-3.png" alt="Chess window selected piece for moving">