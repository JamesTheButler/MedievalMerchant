# Localization: Goods
Strings that describe the various goods and some related info, like regions, abundance, etc.

## Key Organization
- Goods.Availability.*: Availability describes how abundant/scarce a good is. The strings are shown in various places in the UI. They are commonly seen when trading as a modifier to the price of goods (i.e. when hovering the price in the TradeUI)
- Goods.Good.*: Each good has a display name and an associated production building name. Production building names are shown in the bottom right "Production" section of the Town UI. The name is shown all over the places, for example when hovering a good in any inventory.
- Goods.Region.*: Each good is a result from one of 4 regions. Regions translations can be most easily tested by hovering the different regions on the world map.
- Goods.Selector.*: Good selectors tell the player which goods are affected by different modifiers. For example, a level could have lower prices (i.e. a price modifier). Here are a couple of examples for different selectors
  - Lower prices for all goods.
  - Lower prices for hay.
  - Lower prices for berries, wheat and clay.
  - Lower prices for tier II goods from mountains.
  - Lower prices for all goods from mountains, forests and fields.
- Goods.Tootlip.*: Strings are shown in tooltips when hovering goods. These strings should generally be kept short.
