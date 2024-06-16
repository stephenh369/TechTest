# Hello there! 👋
## Here you'll find a walkthrough of my solution to the User Management tech test, where I intend to show each feature i've implemented and some of the issues I ran into along the way.

### 1. Filters Section
The user list page contains 3 buttons beneath the list - **Show All**, **Active Only** and **Non Active**. These buttons now function as filters as intended and will return a list with only the relevant users:

![user-list-unfiltered](https://github.com/stephenh369/TechTest/assets/63809406/fa7dd394-6fbe-4da2-accc-2e261a3646cb)
![active](https://github.com/stephenh369/TechTest/assets/63809406/95efb750-bf41-46f0-829f-bb39c018fe4e)
![non-active](https://github.com/stephenh369/TechTest/assets/63809406/969e3be9-2d4c-4109-967a-e47558ad66ee)

### 2. User Model Properties
`DateOfBirth` property has been added to the `User` class. This can be seen throughout most of the screens within the app from user list, specific user views and add/edit functionality.

### 3. Actions Section
Each CRUD operation has its own view to create the required UI flow to facilitate each operations functionality.

#### Add:
![add-user](https://github.com/stephenh369/TechTest/assets/63809406/d801c0d5-9160-404a-b9c2-ad0a28f9a441)

#### View:
![view-user](https://github.com/stephenh369/TechTest/assets/63809406/112ab505-4e5c-4df3-9f4f-8306c0ccbe2b)

#### Edit:
![edit-user](https://github.com/stephenh369/TechTest/assets/63809406/0952b5cf-fd48-4ff8-ba58-30d7f9df0095)

#### Delete:
![delete-user](https://github.com/stephenh369/TechTest/assets/63809406/768e25ce-6768-424d-8e81-d0d4b2d8c7bd)

#### Validation:
Example of input validation upon POST request with invalid Model values. One issue I had here was changing the error message when there is no value entered into the `DateOfBirth` field to something more user friendly. I had to timebox this and move on but I'd be keen to discuss a potential solution to this.

![validation](https://github.com/stephenh369/TechTest/assets/63809406/116b1a71-4249-4a0b-83d0-91fdc0ffdb4d)

### 4. Data Logging:
`UserActionLog`s are created whenever any CRUD actions are performed with users and displayed in either any given user's specific view, where all relevant logs to that user will show, or in the Logs view which will display all of the logs for all users.

![all-user-logs](https://github.com/stephenh369/TechTest/assets/63809406/51fc8477-2d74-4f2a-b05c-dc4889333dab)
You may also notice that in the case of updates to user information, we are also noting the specific properties that have changed in this action. This information is displayed entirely within the `AdditionalInfo` column of each table row. I think if this functionality were to be expanded upon more to provide some other additional details or a few more properties were added to `User`, then there would be good reason to add an additional view here to provide the full details, and truncate the information provided in the column here. We could also display more user friendly names for the properties.

![user-log](https://github.com/stephenh369/TechTest/assets/63809406/b0019e4b-7a8e-438a-ace8-290293b65456)


![log-pagination](https://github.com/stephenh369/TechTest/assets/63809406/f9120282-d5f4-4ad9-b9fc-b7652baa4af4)
As this page would quickly begin to fill up if the app were using persistent data, i've implemented pagination in this view, splitting the results into sets of 10 per page.

### Unit Tests
I began to implement unit tests as part of this solution but quickly ran into a few issues and realised I likely wouldn't have the time to scope these out as much as I'd have liked, so they are currently in a state of WIP.
The `DataContext` tests should all pass as expected.
The `UserServiceTests` and `LogsServiceTests` both had issues where finding specific entities by id would return null and so currently still fail. My current assumption is that these are problems specifically with the tests.
The `UserControllerTests` had the null issue specifically with deletion, others are passing.

I'd love to discuss these further and decipher where I went wrong here, as it was here I felt I probably spent the most time with issues. 

### Some additional context
While I have some previous experience with Java, OOP and MVC frameworks as a concept, I currently work predominantly within the JS ecosystem. This is the first time I have worked with .NET, and so I have spent a lot of time reading documentation to learn the framework and learn the C#/.NET equivalents of various methods and features. I say this mainly in hope that I've demonstrated that I'm able to use my previous experience to get up to speed fairly quickly with new tools.

Additionally, I'd like to state that I'm keen to move into the C#/.NET ecosystem to grow my skillset as a developer and continue to push and challenge myself.

Would love to discuss this solution further, look forward to hearing your feedback!
