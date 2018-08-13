# Untile
CLI tool to untile tiled images typically found in tile sets

#### Usage: Untile [options]

**Options:**  
  -i|--input <INPUT>    The input image to process  
  -o|--output <OUTPUT>  The output path to extract the images to, if no path is given defaults to working directory  
  -t|--tiles <TILES>    The size each tile. i.e. 16x16  
  -?|-h|--help          Show help information  
  
*Example:*  
`dotnet run -- --input ~/Projects/IsoVoxel/Art/TileSet/tiles_dungeon_v1.1.png --output ~/Projects/IsoVoxel/Art/TileSet/2D --tiles 16x16`
