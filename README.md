# MessageLogger, MVC Application

### Setup
* Clone repository
* Run `update-database` in the package manager console.
    * **NOTE:** There is no seed data for this project.
* Launch!

### About
* MvcMessageLogger is an application that allows users to create an account, and log messages that they can go back to and edit, or delete.
* Additionally, users are able to edit their own profile.
* All changes made by the user are saved to a local database.
* I took quality time to plan ahead for this project, and [here](https://docs.google.com/presentation/d/1tTMBGe0dp3r7w804kxL6c7oSsonqnPCd7bZ1tLILHik/edit?usp=sharing) you can find that planning VS. actual.

### Tech Stack
#### Front-End
  * Blazor (HTML/CSS)

  #### Back-End
  * C#
  * SQL

  #### Frameworks Managed by NuGet
  * ASP.NET MVC
  * Entity Framework
  * xUnit Tests

### Wins
* Sucessfully implemented CRUD actions for both `users` and `messages`
* Used cookies to maintain a user's state, allowing them to access their profile page from all views.
* Implemented bootstrap files - mixed with custom CSS to create a unique viewing experience.

### Challenges
* The largest challenge I ran into was cleanly merging the bootstrap with my custom CSS
    * Until this point, I had not used much HTML/CSS so I was reaching into unfamiliar territory

### Context & Goal
* This was a solo assignment from the Launch program at [Turing](https://launch.turing.edu/), which I had three days to work on.
* My goal was to be able to perform CRUD (Create, Read, Update, Delete) actions on related resources - in this case between `users` and `messages`
