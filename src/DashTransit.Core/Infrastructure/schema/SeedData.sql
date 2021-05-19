USE dashtransit;
GO

DELETE FROM Messages
INSERT INTO Messages
    (Id, CorrelationId, Content, Type, Timestamp)
VALUES
    ('2d6ee703-c0f5-493b-83c4-663cf8cecca7', NEWID(), CONCAT('{"job":"', NEWID(), '"}'), 'Message1', GETDATE()),
    ('fab6de5a-5349-4278-b184-63dc26aacc78', NEWID(), CONCAT('{"job":"', NEWID(), '"}'), 'Message2', GETDATE())

DELETE FROM Faults
INSERT INTO Faults 
    (Id, MessageId, Exception, StackTrace, Type, Source)
VALUES
    (NEWID(), 'fab6de5a-5349-4278-b184-63dc26aacc78', 'Bad thing', 'Line 1', 'System.InvalidOperationException', 'Place')

DELETE FROM Endpoints
INSERT INTO Endpoints
    (Id, Uri)
VALUES
    (1, 'web'),
    (2, 'api'),
    (3, 'worker')

INSERT INTO Sent
VALUES
    ((SELECT $node_id
        FROM Endpoints
        WHERE ID = 1),
        (SELECT $node_id
        FROM Messages
        WHERE Id = 'fab6de5a-5349-4278-b184-63dc26aacc78')),
    ((SELECT $node_id
        FROM Endpoints
        WHERE ID = 2),(SELECT $node_id
        FROM Messages
        WHERE Id = '2d6ee703-c0f5-493b-83c4-663cf8cecca7'))


INSERT INTO Recieved
VALUES
    ((SELECT $node_id
        FROM Endpoints
        WHERE ID = 3),(SELECT $node_id
        FROM Messages
        WHERE Id = 'fab6de5a-5349-4278-b184-63dc26aacc78')),
    ((SELECT $node_id
        FROM Endpoints
        WHERE ID = 3),(SELECT $node_id
        FROM Messages
        WHERE Id = '2d6ee703-c0f5-493b-83c4-663cf8cecca7'))

