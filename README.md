15-Days-15-Games-Day-15-Whack-a-Food
This is the fifteenth and final game from my "15 Days 15 Games" challenge. It is a 2D/3D "Whack-a-Mole" style arcade game featuring a grid-based spawning system, dynamic difficulty, and a time-attack game loop.

🚀 About the Game
The game is played on a 4x4 grid. Objects (food and hazards) appear in random grid squares for a short duration. The player must click on "good" items to score points and avoid "bad" items, which deduct points. The game is on a 60-second (or adjustable) timer. The goal is to achieve the highest score possible before time runs out. The game's spawn speed is set by a difficulty level chosen on the start screen.

💡 Technical Highlights
Engine: Unity

Grid-Based Spawning System: The GameManagerX and TargetX scripts implement a precise grid-based spawning system. A minValueX, minValueY, and spaceBetweenSquares float are used to calculate an array of 16 possible spawn positions. Objects are instantiated at new Vector3(minValueX + (RandomSquareIndex() * spaceBetweenSquares), ...) ensuring they always appear perfectly centered within the grid squares.

Time-Attack Game Loop: The game's primary loop is controlled by a remainingTime float in the GameManagerX. This timer counts down every frame. When it reaches zero, GameOver() is called. The timer is displayed in a 00:00 format using string.Format("{0:00}:{1:00}", minutes, seconds).

Dynamic Difficulty: The DifficultyButtonX script passes an int difficulty to the GameManagerX.StartGame() method. This integer is used as a divisor for the spawnRate (spawnRate /= difficulty), directly increasing the game's speed and challenge.

Data-Driven Prefabs: A single TargetX.cs script is used for all spawned objects. By configuring pointValue in the Inspector and setting the object's tag to "Bad", both good and bad items are created from the same reusable, data-driven prefab.

Polished Player Feedback: The game provides excellent feedback, including a RedFlashRoutine coroutine in the GameManagerX that flashes a red UI overlay when the player clicks a bad item. Particle effects and audio (AudioSource.PlayClipAtPoint) are triggered by the TargetX script upon being clicked.

▶️ Play the Game!
You can play the game in your browser on its itch.io page: []
