IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Id = 12345)
BEGIN
    INSERT INTO Usuarios
    (
        Id,
        Nome,
        Email,
        SenhaHash,
        Perfil,
        Notificar,
        CreateAt,
        UpdateAt,
        Ativo
    )
    VALUES
    (
        12345,
        'adminCarometro',
        'admin@carometro.info',
        '10000.wCHnjB7/BBCzCzIR5GYbnw==.A4vPmmNApGil1tqd3LlsgbbBBrrz1GT2S1yLoemkous=',
        1,
        0,
        GETDATE(),
        GETDATE(),
        1
    );
END;