CREATE TABLE Endpoints
(
    Idx INTEGER PRIMARY KEY IDENTITY(1,1),
    Id UNIQUEIDENTIFIER,
    Uri VARCHAR(512)
);
CREATE INDEX Idx_Id ON Endpoints(Id)

CREATE TABLE MessageTypes
(
    Idx INTEGER PRIMARY KEY IDENTITY(1,1),
    Id UNIQUEIDENTIFIER,
    Name VARCHAR(512)
);
CREATE INDEX Idx_Id ON MessageTypes(Id);

CREATE TABLE Messages
(
    MessageId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ConversationId UNIQUEIDENTIFIER NOT NULL,
    Timestamp DATETIMEOFFSET NOT NULL,
    Content VARCHAR(MAX) NOT NULL,
    MessageTypeId INT NOT NULL,
    SourceEndpointId INT,
    DestinationEndpointId INT,
    CONSTRAINT FK_MessageType FOREIGN KEY (MessageTypeId) REFERENCES MessageTypes(Idx),
    CONSTRAINT FK_SourceEndpointId FOREIGN KEY (SourceEndpointId) REFERENCES Endpoints(Idx),
    CONSTRAINT FK_DestinationEndpointId FOREIGN KEY (DestinationEndpointId) REFERENCES Endpoints(Idx)
) AS NODE;
CREATE INDEX Idx_Timestamp ON Messages(Timestamp)

CREATE TABLE ExceptionTypes
(
    Idx INTEGER PRIMARY KEY IDENTITY(1,1),
    Id UNIQUEIDENTIFIER,
    Name VARCHAR(512)
);
CREATE INDEX Idx_Id ON ExceptionTypes(Id)

CREATE TABLE Faults
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    MessageId UNIQUEIDENTIFIER NOT NULL,
    Exception VARCHAR(MAX) NOT NULL,
    StackTrace VARCHAR(MAX),
    Source VARCHAR(1000),
    ExceptionTypeId INT NOT NULL,
    CONSTRAINT FK_ExceptionType FOREIGN KEY (ExceptionTypeId) REFERENCES ExceptionTypes(Idx),
    CONSTRAINT FK_MessageFault FOREIGN KEY (MessageId) REFERENCES Messages(MessageId)
);

CREATE TABLE Initiator(ConversationId UNIQUEIDENTIFIER NOT NULL) AS EDGE;
GO