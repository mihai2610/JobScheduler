# JobScheduler
Scheduler to solve jobs async

# Setup

Solution requires .net 6 sdk  <br />
Solution uses SQLiteDb follow the site for more details https://www.sqlite.org/about.html  <br />
Solution uses RabbitMq as message brocker, in order to run it an instance is required  <br />
You can use:
  ```
  docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.10-management
  ```

# Architecture

![image](https://user-images.githubusercontent.com/14985894/182467326-5e4bedd8-bb02-4ee9-8c4b-499e7b6a6100.png)

#### Api
  Contains controllers and view models
  
#### Domain
  Layer that contains business logic and models

#### Infrastructure
  Layer that performs all the interactions with external tools

#### External Packages
  External clients are implemented here and are used in the infrastructure layer

#### Worker
  Every 5 seconds reads messages from the Queue, performes the job and store the result in the database

  # Solution
  
  ![image](https://user-images.githubusercontent.com/14985894/182472226-1dc10b84-124d-45fd-bb64-d26cade12f3d.png)
