# User Stories


|Q-001: Primary Contact Question|
|:---|
|*User Story* 
The Primary Contact is asking why it is needed to use the `/Login` endpoint before using the `/Item/Entry` endpoint, and why not use only the `/Item/Entry` endpoint. Currently the system behavior gives and error when using only the `/Item/Entry` endpoint|
|*Points*: |
|*Steps to reproduce*
Checkout to the main branch and follow:
First run the `/Item/Entry` endpoint as you are increasing the quantity (you can use the code snippet below) for an item in a facility. Note it will throw an error.
Now run the `/Login` endpoint and then run the `/Item/Entry` endpoint, note the process was successfully|
|*Dev task* 
Explain to the Primary Contact what is happening with the current behavior about the error and why it is needed to run the `Login` endpoint to this to work.
To complete this task, send a Teams message to __Rodrigo Alarcón__ since he is he Primary Contact, and explain (bouns points if in English). Please note that no code change is needed.|

Code snippet
```json
{
  "itemCode": "PANT-001",
  "destinationType": 0,
  "destinationId": 1,
  "additionalQuantity": 1000,
  "description": ""
}
```


|D-001: Now way to add or update a Warehouse|
|:---|
|*User Story* 
Just receiving error every time I want to update or insert a new warehouse. Use the Post and Put Endpoints for Warehouse, see they give different errors.|
|*Points*: |
|*Expected Behavior* 
The warehouses should be updated or inserted without errors|


|D-002: Not able to increase the quantity of an item in a Warehouse|
|:---|
|*User Story* 
Not able to increase the quantity of an item in a Warehouse. When using the Post: /Item/Increase endpoint receive error.|
|*Points*: |
|*Expected Behavior* 
The user should be able to increase the quantity in Warehouses or Facilities.|


|F-003: Warehouse Reporting|
|:---|
|*User Story* 
As a user I want to run a report of the state of a given warehouse, I want to see all the available items with their quantities, and only show the ones that have a quantity above 0|
|*Points*: |
|*Expected Behavior* 
Create a new endpoint to male a Warhorse reporting. Should only return items with quantity more than 0|

|D-003: Get all facilities doesn't return the correct data|
|:---|
|*User Story* 
When running the Get All Facilities endpoint it just return a empty object|
|*Points*: |
|*Expected Behavior* Should return the correct data|

|D-004: Not allow to have negative inventory|
|:---|
|*User Story* 
none|
|*Points*: |
|*Expected Behavior* 
The inventory shouldn't be open to have negative values|

|F-004: Inventory Outcomes|
|:---|
|*User Story* 
none|
|*Points*: |
|*Expected Behavior* 
Should be the option to make outcomes of items from Warehouse and Facilities|

|F-005: Inventory movements|
|:---|
|*User Story* 
As a regular user I want to be able to make movements from Warehouses to Facilities or vice versa.|
|*Points*: |
|*Expected Behavior* 
Should be the option to make movements between Warehouses and Facilities.|

|F-006: Offices|
|:---|
|*User Story* 
Many of our target public are using Offices that works as the primary place to make Sales Orders. Our system should be able to reach this by having three types of Offices: Large, Medium and Small, Large Offices can support a storage of 200 items, Medium Offices can storage 125 items and Small Offices can storage 50 items. |
|*Points*: |
|*Expected Behavior* 
The system should support Offices as a place to store items as Warehouses or Facilities but with quantity limitation.
In a future Offices are planned to be the only place to make Sales Orders (See Ticket F-011) |
|*Dev Moves* 
Add a new table called `tnoffc` as the primary table for Office, and add another called `tnitmofc` as the linking table for Items and Offices |


|F-011: Sales Orders|
|:---|
|*User Story* 
The system should be able to perform Sales Orders, Sales Orders can have place only in Offices and every SO data should be stored in the database for future calculations |
|*Points*: |
|*Expected Behavior* 
The system should support Sales Orders, SO should reduce the items in Offices, if the items inside a Office is less than the required amount the System should send a message that there is no enough items. Additional points: create a custom numeration for every SO, this so_number field should be different than the regular primary key |
|*Dev Moves* 
Add a new table called `tnssord` as the primary table for SO
Add a new table called `tnitprce` that will hold the prices for the items, if an item has no price the default price should be $US 15 |

|F-012: Selling pants endpoint|
|:---|
|*User Story*
As a regular employee I want an endpoint to sell pants in an specific facility, this endpoint should be able to receive a list of items and the quantity of each item, This should take into account all the requirements listed above.|
|*Points*: |
|*Expected Behavior*
The endpoint should create a new Sales Order and add the items to it, the system should send a message when there is no enough items in the Office. Additional points: be able to transfer items from a warehouse to the Office|
|*Dev Moves*
Add an endpoint to create a sales order, this should return the SO number and the total price|


|(Optional) F-001: System Feedback for Errors|
|:---|
|*User Story* 
As a final user I want to receive a feedback from the system every time an error has ocurred, instead of the big garbage of data that I'm currently receiving, in this way I will have a basic knowledge of what happened.|
|*Points*: |
|*Expected Behavior*
Every time an error has happened, a simple message error should be returned. if it is necessary explain to the user the illegal action has caused this exception.|
## User Stories Resolution

In your commit after solving a defect or implementing a new feature, the ticket ID should be first followed by your commit message.

Example:

`git commit -m "F-001 Added custom exception for unknown errors"`

If you're going to make changes not related with some User Story/ticket, just add you commit message explaining what you did.

