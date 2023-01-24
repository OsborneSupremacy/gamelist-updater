# GAMELIST UPDATER

This is a utility designed to make bulk updates (at this point, deletes only) to gamelist.xml files associated with
[Emulation Station](https://emulationstation.org/).

If you use Emulation Station via [Retro Pie](https://retropie.org.uk/) or other means, you may find that your collection of legitimate ROMs contains a lot of games that you don't really want. Possible reasons:

* They're age inappropriate (or inappropriate for any age)
* They're types of games you're not interested in
* They're just bad games
* All of the above

Emulation Station, by design, doesn't offer a fast way of deleting games. And if you're deleting games from the file system directly, that is  cumbersome because all you see there are file names which don't convey much info.

What I've found is that the "red flag" that warrants a game's deletion could be in the name, genre, publisher, description, or elsewhere. So I created this program that lets you programmatically identify and delete games, in bulk, using that data.

## How to Use

### Scrape First

This program depends on populated gamelist.xml files. By default, when you import a rom into Emulation Station, its entry in gamelist.xml will look something like this:

```xml
<game id="0" source="ScreenScraper.fr">
  <path>./scramble.zip</path>
  <name>scramble</name>
  <desc />
  <releasedate />
  <developer />
  <publisher />
  <genre />
  <players />
</game>
```

That is nothing to go on, so for an entry like that, this program isn't going to do anything (unless the specific name was blacklisted).

What you want do to is populate that XML data for all of your games using a scraping utility. I highly recommend [Skraper](https://www.skraper.net/). It is the most accurate and reliable utility I've used.

Once you've run that, a game's entry in gamelist.xml will look something like:

```xml
<game id="42951" source="ScreenScraper.fr">
  <path>./seawolf.zip</path>
  <name>Sea Wolf (set 1)</name>
  <desc>The game screen is a side view of a underwater scene (with the surface towards the top)...</desc>
  <rating>0.7</rating>
  <releasedate>19760101T000000</releasedate>
  <publisher>Dave Nutting Associates</publisher>
  <genre>Shooter / Space Invaders Like-Shooter</genre>
  <players>1</players>
  <image>./media/images/seawolf.png</image>
  <marquee>./media/marquee/seawolf.png</marquee>
  <video>./media/videos/seawolf.mp4</video>
  <genreid>260</genreid>
</game>
```

With that data populated, we can proceed to delete games meeting various criteria.

### Configure

You're going to want to open and customize this program's [appsettings.json](GameXmlReader/appsettings.json) file.

Here are the setting to adjust:

#### FlagWhenAdultFieldIsTrue

The `game` XML object sometimes contains an `adult` node. If this setting is set to `true`, games with a value of `true` in the adult node will be flagged.

#### Target

```json
"Target": {
  "Directory": "\\\\RETROPIE\\roms\\mame-libretro",
  "Xml": "\\\\RETROPIE\\roms\\mame-libretro\\gamelist.xml"
}
```

A couple of things to note:

1. The application is designed to look at one system's rom at a time.
2. The application will need read and write access to the rom folder.

You can verify the file paths in a file explorer window. If "RETROPIE" doesn't work, use the IP address directly.

#### FlaggedTerms

The flagged terms currently in appsettings are my preferences only. For example, I'm totally find deleting every Mahjong-related game. If you're not, you're going to want to remove that. Customize the terms as your desire. As you'll notice, you can customize keywords in rom names, genres, descriptions (the "words" section), and publishers.

### Run

The program will ask you to confirm before it deletes anything. You will be able to confirm exactly which games it deletes, before it deletes them.

If the program does not find any games containing any flagged terms, it will exit without doing anything.

If it does, it will create three files in a temporary directory on your computer. When it does this, browse to that directly and review the files. The files are:

* `gamelist-pre-update.xml` - this is a copy of the current gamelist.xml, before you update it (which you don't have to do).
* `gamelist-flagged.xml` - this is a list of only the flagged games. If games are in the the list that you don't want deleted, do not commit the operation.
* `gamelist-new.xml` - this is what gamelist.xml will be updated to, if you commit the operation.

If you commit the update, the program will try to delete the rom and any media files associated with it.

## Contribute

Please feel free to contribute to this repository. At this point it is a very quick and dirty app designed for my immediate use. I'm sure it can be updated to do a lot of other useful things.

![It does other things (eventually)](it-does-other-things.gif)
