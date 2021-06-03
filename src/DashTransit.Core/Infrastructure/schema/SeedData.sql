USE dashtransit;
GO

DROP TABLE IF EXISTS #CoversationIds
CREATE TABLE #CoversationIds (c UNIQUEIDENTIFIER)
GO

INSERT INTO #CoversationIds (c) 
SELECT NEWID() 
GO 100

DELETE FROM Messages
GO
INSERT INTO Messages (MessageId, Content, Timestamp, ConversationId)
SELECT NEWID(), CONCAT('{"job":"', NEWID(), '"}'), GETDATE(), (SELECT TOP 1 c FROM #CoversationIds ORDER BY NEWID())
GO 500

DELETE FROM Endpoints
GO
INSERT INTO Endpoints
    (Uri)
VALUES
    ('web'),
    ('api'),
    ('worker')

DELETE FROM MessageTypes
GO
INSERT INTO MessageTypes
    (Name)
VALUES  
    ('Message1'),
    ('Message2'),
    ('Message3'),
    ('Message4'),
    ('Message5')
GO

;WITH Vals AS
(
    SELECT  m.$node_id as m, t.$node_id as t,
        ROW_NUMBER() OVER( PARTITION BY m.$node_id ORDER BY NEWID()) RowNumber
    FROM Messages m, MessageTypes t
)
INSERT INTO OfType
SELECT  m, t
FROM    Vals
WHERE   RowNumber <= 1
GO

;WITH Vals AS
(
    SELECT  m.$node_id as m, t.$node_id as t,
        ROW_NUMBER() OVER( PARTITION BY m.$node_id ORDER BY NEWID()) RowNumber
    FROM Messages m, Endpoints t
)
INSERT INTO Source
SELECT m,t
FROM    Vals
WHERE   RowNumber <= 1
GO

;WITH Vals AS
(
    SELECT  m.$node_id as m, t.$node_id as t,
        ROW_NUMBER() OVER( PARTITION BY m.$node_id ORDER BY NEWID()) RowNumber
    FROM Messages m, Endpoints t
)
INSERT INTO Destination
SELECT m,t
FROM    Vals
WHERE   RowNumber <= 1
GO

;WITH m AS
(
    select
         ROW_NUMBER() OVER( PARTITION BY ConversationId ORDER BY NEWID()) RowNumber,
         ConversationId,
         MessageId
    from Messages
),
last_message AS
(
    SELECT MAX(RowNumber) as RowNumber, ConversationId
    FROM m
    GROUP BY ConversationId
),
c AS
(
    SELECT m.RowNumber, MessageId, m.ConversationId, CAST(NULL AS uniqueidentifier) as Initiator, CAST(NULL AS bigint) as InitiatorIndex
    FROM m
    INNER JOIN last_message l ON l.ConversationId = m.ConversationId AND l.RowNumber = m.RowNumber
    UNION ALL
    SELECT m.RowNumber, m.MessageId, m.ConversationId, c.MessageId as Initiator, c.RowNumber as InitiatorIndex
    FROM m
    INNER JOIN c ON m.RowNumber = c.RowNumber - 1 AND m.ConversationId = c.ConversationId
    WHERE m.RowNumber >= 1
),
i AS
(
    select m.$node_id AS MessageNode, i.$node_id AS InitiatorNode, c.ConversationId from c
    inner join Messages m on c.MessageId = m.MessageId
    inner join Messages i on c.Initiator = i.MessageId
)
INSERT INTO Initiator
SELECT MessageNode,InitiatorNode,ConversationId
FROM i
GO

DROP TABLE IF EXISTS #CoversationIds

