# Localization: Tutorial
Tutorial strings are divided into two main groups.


## 1. Onboarding
Strings with keys that start with 'Tutorial.Onboarding.' are used in the introduction level, when players start the game for the first time. The intro can be replayed from the start screen by clicking the "Replay Intro" button in the bottom-right.

Onboarding strings are either tasks or explainers.

### Tasks
Indiciated by keys like "Tutorial.Onboarding.Task...."

A Task is a string shown in the task list UI on the top of the screen, which must be checked off by the player, e.g. "Sell 25 sheep in Berrytown".
Keep these tasks short as they must fit onto the relatively small task lsist UI in the top of the screen.

Tasks heavily use placeholders. Make sure to copy these over to the translation exactly.

### Explainer
Indiciated by keys like "Tutorial.Onboarding.Explainer..."

An explainer is a longer text shown to the player to explain game mechanics and goals. It's shown in the popup with the "continue" button and doesn't need to be concise as the popup is dynamically sized and therefore is quite flexible.

Explainers mainly use placeholders for town names. In the intro level, there are two towns which are marked with {TownA} and {TownB} in the text. It's important that you don't mix up TownA and TownB in your translations as that would confuse the player a lot and break the intro flow.


## 2. Tutorial Topics

Strings with keys start start with "Tutorial.Topic..." are used in the tutorial UI in-game which is shown to the player by clicking the [?] buttons in various areas in the UI.
Tutorial topics always have
- a title, shown at the top of the tutorial UI and when hover [?] buttons
- and a collection of chapters, indicated by the ui element that shows for example [1/5] lower-right corner of the image.

Tutorial topic chapters can be viewed through by clicking the [<] and [>] buttons in the tutorial ui, where each step contains
- an image
- a title; shown below the image 
- a description; shown in the main text box in the bottom

You can interpret the keys for these strings like so:
Tutorial.Topic.Intro.Chapter.Trading.Title
means that we are talking about the chapter about "Trading" in the "Intro" topic, so it's
Tutorial.Topic.<TopicName>.Chapter.<ChapterName>.Title

Tutorial topics are fully static texts. No placeholder should be in there. If they are, let me know.

### Testing

To easily view and test all tutorial topics, open the scene called 'TutorialTests' in the Project-view search-bar by double-clicking it. You can then select each tutorial topic in the drop-down menu and inspect each chapter.