﻿using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Disposable wrapper for a database connection (the connection will be closed if it was opened by this class, but will remain opened if it was already opened)
    /// </summary>
    public class DBOracleConnection : IDisposable
    {
        #region Properties

        /// <summary>
        /// Database connection
        /// </summary>
        internal DBOracleConnectionInstance Connection { get; set; }

        /// <summary>
        /// Indicates if the database connection should be closed and disposed when this object is disposed
        /// </summary>
        private bool DisposeConnection { get; set; }

        /// <summary>
        /// Indicates if the database connection is opened
        /// </summary>
        public bool Connected => Connection?.Connected ?? false;

        /// <summary>
        /// Indicates if there is an active transaction for this database connection. Returns false if the connection is not opened.
        /// </summary>
        public bool HasTransaction => Connection?.HasTransaction ?? false;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of DBOracleConnection
        /// </summary>
        protected DBOracleConnection()
        {
        }

        #endregion

        #region Create new connection wrapper

        /// <summary>
        /// Creates a disposable container with a database connection, creating a new connection if necessary
        /// </summary>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        protected static T Create<T>(DBOracleConnection connection, string connectionStringName) where T : DBOracleConnection
        {
            var newConnection = (T)Activator.CreateInstance(type: typeof(T), nonPublic: true);

            if (connection is null)
            {
                newConnection.DisposeConnection = true;
                newConnection.Connection = DBOracleConnectionInstance.Open(connectionStringName);
            }
            else
            {
                newConnection.DisposeConnection = false;
                newConnection.Connection = connection.Connection;
            }

            return newConnection;
        }

        /// <summary>
        /// Creates a disposable container with a database connection, creating a new connection if necessary
        /// </summary>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        public static DBOracleConnection Create(DBOracleConnection connection, string connectionStringName)
        {
            var newConnection = new DBOracleConnection();

            if (connection is null)
            {
                newConnection.DisposeConnection = true;
                newConnection.Connection = DBOracleConnectionInstance.Open(connectionStringName);
            }
            else
            {
                newConnection.DisposeConnection = false;
                newConnection.Connection = connection.Connection;
            }

            return newConnection;
        }

        #endregion

        #region Create transaction

        /// <summary>
        /// Creates a transaction wrapper, beginning a new transaction if necessary
        /// </summary>
        /// <param name="createSavepoint">Indicates if a savepoint should be created if there is already an active transaction</param>
        /// <param name="isolationLevel">Transaction isolation level, only <see cref="IsolationLevel.ReadCommitted"/> and <see cref="IsolationLevel.Serializable"/> are supported by Oracle managed client (default is <see cref="IsolationLevel.ReadCommitted"/>)</param>
        /// <returns>New transaction wrapper</returns>
        public DBOracleTransaction BeginTransaction(bool createSavepoint = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return Connection?.BeginTransaction(createSavepoint: createSavepoint, isolationLevel: isolationLevel)
                ?? throw new InvalidOperationException(DBOracleLocalizedText.DBOracleConnection_BeginTransaction_Closed);
        }

        #endregion

        #region Close database connection

        /// <summary>
        /// Dispose the database connection if applicable
        /// </summary>
        internal virtual void Close()
        {
            if (DisposeConnection)
            {
                // DBOracleConnection.Close wont throw exceptions
                Connection?.Close();
            }

            Connection = null;
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// Indicates if this instance was already disposed, to detect redundant calls.
        /// This is part of the default IDisposable implementation pattern.
        /// </summary>
        private bool DisposedValue = false;

        /// <summary>
        /// Internal implementation of the dispose method, to release managed and/or
        /// non-managed resources, as necessary.
        /// </summary>
        /// <param name="disposing">True if the Dispose(bool) method is being called
        /// during the diposal of this instance (by calling Dispose() or at the end
        /// of using clause), or false if this method is being called by the finalizer
        /// of this class. This is part of the default IDisposable implementation
        /// pattern.</param>
        private void Dispose(bool disposing)
        {
            if (DisposedValue)
                return;

            try
            {
                // Dispose managed resources

                if (disposing)
                {
                    Close();
                }

                // We don't have non-managed resources to be released.
            }
            finally
            {
                DisposedValue = true;
            }
        }

        /// <summary>
        /// Public implementation of IDisposable.Dispose(), to be called explicity by
        /// the application, or implicity at the end of the using clause. This is part
        /// of the default IDisposable implementation pattern.
        /// </summary>
        public void Dispose()
        {
            // Dispose managed and non-managed resources
            Dispose(true);
        }

        #endregion
    }
}
