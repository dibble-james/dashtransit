# DashTransit

Gain an insight into your MassTransit installation

[![CI Build](https://github.com/dibble-james/dashtransit/actions/workflows/ci.yml/badge.svg?branch=trunk)](https://github.com/dibble-james/dashtransit/actions/workflows/ci.yml)


## Features

- [x] View your audit logs
- [x] See related messages
- [x] See the endpoints associated with a message
- [x] Analyse faults
- [x] Re-send failed messages
- [x] Send new message
- [x] Edit and re-send

![Screenshots](https://user-images.githubusercontent.com/11923585/159138708-607e4984-ba1f-4b7b-b87f-beef90292fe0.png)

### Coming soon...

- [ ] Search audit logs
- [ ] Improved endpoint details
- [ ] Azure Service Bus

## Supported Configurations

More to follow but for now we support:

| Transport |
| --------- |
| RabbitMQ  |

| Storage    | Provider Variable |
| ---------- | ----------------- |
| SQL Server | sqlserver         |
| Postgres   | postgres          |

## Getting started

DashTransit is distributed as a Docker container. Both Linux and Windows containers are supported.

You can create the tables DashTransit requires by migrating using:

```
docker run --name dashtransit-migrations -e store__provider=<PROVIDER VARIABLE> -e store__connection= ghcr.io/dibble-james/dashtransit migrate
```

Just provide connection strings for the storage and transport. The web app is exposed on port 80.

```
docker run -d -p <desired port>:80 --name dashtransit -e transport__connection= -e store__provider=<PROVIDER VARIABLE> -e store__connection= ghcr.io/dibble-james/dashtransit
```

Then enable [Message Audit](https://masstransit-project.com/advanced/audit.html) on all your endpoints and DashTransit will start to consume any faults raised and will now have access to the audit logs created by MassTransit.
