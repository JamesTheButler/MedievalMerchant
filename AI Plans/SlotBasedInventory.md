My inventory data structure () is generic. It's just a dict<Good, int>. The Player, the player's camp and the town class all have inventories.
Their specific rules are handled via IInventoryPolicy classes. E.g. towns are allowed to have goods based on the town and good tier, the player only has a limited amount of slots, etc.

I need to improve the code to represent which cart has which goods from the player inventory in which slot. Right now, there is no code for this in the model layer. The CaravanPanelUI class just manages the UI inventory cells by hand. This is a problem, because i need a different UI to also show the contents of the player inventory in the carts and it needs to match. Furthermore, there are bugs like the inventory not being filled in the right order (i.e. cart 1, slot 1, slot 2, ..., all the way to cart 4 slot 4, in order). Right now, slots are filled in the order of being upgraded, which is not expected behavior.

I have started two approaches, both with their own issues. I wrote an InventorySlotMapper, which is supposed to map goods from the inventory to a specific free slot. I don't like this approach. I would much rather have the Cart class own its own slots and allow observable access to each slot so that the UI can simply bind each inventory cell to specific carts' slots.

What do you think about my current approaches?
Is there anything left I need to explain to you, gaps in my explanations or confusing details?

Outline a plan, WITHOUT CONCRETE CODE to help me understand the right architecture and correct responsibility ownership between the classes, i.e. who knows what and how do we not doplucate too much state (single source of truth).

Relevant files:
Assets/Features/Player/Caravan/UI/CaravanPanelUI.cs
Assets/Features/Inventory/Inventory.cs
Assets/Features/Player/Caravan/Logic/Cart.cs
Assets/Features/Player/Logic/InventorySlotMapper.cs