using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using Mono.Data.Sqlite;
using System;

public abstract class ACEntity {

    protected SqliteConnection conn = DBConnection.getDbConnection();
    protected Validator validator = Validator.getInstance();


}
