# SignalR.Auth.Demo

1. Run SignalR.Server
2. Run SignalR.Client
3. Use console commands such as:
 - `I am <Username>`
 - `Send <Message Text>`
 - Logout
 
 You should be able to reproduce following input/output:
 
```
I am Vlad
Logged you in as Vlad with jwt eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.whocares
Send something
Received message from Vlad: something
Logout
I am David
Logged you in as David with jwt eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.whocares
Send whatever
Received message from David: whatever
```
