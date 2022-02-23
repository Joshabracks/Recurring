namespace Gameplay.Terrain {
    public enum TerrainType {
        Sand,
        Dirt,
        Grass,
        Water,
        Hole,
        Rock,
        QuickSand,
        Lava,
    }

    public class BlockingTerrainType {
        public TerrainType[] Types = new TerrainType[] {
            TerrainType.Water,
            TerrainType.Hole
        };
    }
}
