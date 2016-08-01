using System;
using System.IO;

namespace BoardAutoTesting.Log
{
    /// <summary>
    ///     An object that provides basic logging capabilities.
    ///     Copyright (c) 2006 Ravi Bhavnani, ravib@ravib.com
    ///     This software may be freely used in any product or
    ///     work, provided this copyright notice is maintained.
    ///     To help ensure a single point of release, please
    ///     email and bug reports, flames and suggestions to
    ///     ravib@ravib.com.
    /// </summary>
    internal class EzLogger
    {
        #region Helper methods

        /// <summary>
        ///     Writes a log message.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool WriteLogMsg
            (Level level,
                string msg)
        {
            lock (this)
            {
                // Fail if logger hasn't been started
                if (LoggerState == State.Stopped)
                    return false;

                // Ignore message logging is paused or it doesn't pass the filter
                if ((LoggerState == State.Paused) || ((_levels & (uint) level) != (uint) level))
                    return true;

                // Write log message
                var tmNow = DateTime.Now;
                var logMsg = string.Format("{0} {1}  {2}: {3}",
                    tmNow.ToShortDateString(), tmNow.ToLongTimeString(),
                    level.ToString().Substring(0, 1), msg);
                _logFile.WriteLine(logMsg);
                System.Diagnostics.Debug.WriteLine(logMsg);
                return true;
            }
        }

        #endregion

        #region Attributes

        /// <summary>
        ///     Log levels.
        /// </summary>
        [Flags]
        public enum Level
        {
            /// <summary>Log debug messages.</summary>
            Debug = 1,

            /// <summary>Log informational messages.</summary>
            Info = 2,

            /// <summary>Log success messages.</summary>
            Success = 4,

            /// <summary>Log warning messages.</summary>
            Warning = 8,

            /// <summary>Log error messages.</summary>
            Error = 16,

            /// <summary>Log fatal errors.</summary>
            Fatal = 32,

            /// <summary>Log all messages.</summary>
            All = 0xFFFF
        }

        /// <summary>
        ///     The logger's state.
        /// </summary>
        public enum State
        {
            /// <summary>The logger is stopped.</summary>
            Stopped = 0,

            /// <summary>The logger has been started.</summary>
            Running,

            /// <summary>The logger is paused.</summary>
            Paused
        }

        #endregion

        #region Construction/destruction

        /// <summary>
        ///     Constructs a EZLogger.
        /// </summary>
        /// <param name="logFilename">Log file to receive output.</param>
        /// <param name="bAppend">Flag: append to existing file (if any).</param>
        /// <param name="logLevels">Mask indicating log levels of interest.</param>
        public EzLogger
            (string logFilename,
                bool bAppend,
                uint logLevels)
        {
            _logFilename = logFilename;
            _bAppend = bAppend;
            _levels = logLevels;
        }

        /// <summary>
        ///     建立一条记录，默认路径为C盘。
        /// </summary>
        /// <param name="bAppend"></param>
        /// <param name="logLevels"></param>
        /// <param name="filename"></param>
        public EzLogger(bool bAppend, uint logLevels, string filename = "D:\\BoardTest_")
        {
            _logFilename = filename + DateTime.Now.ToString("yyyy_M_dd") + ".log";
            _bAppend = bAppend;
            _levels = logLevels;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets and sets the log level.
        /// </summary>
        public uint Levels
        {
            get { return _levels; }
            set { _levels = value; }
        }

        /// <summary>
        ///     Retrieves the logger's state.
        /// </summary>
        public State LoggerState
        {
            get { return _state; }
        }

        #endregion

        #region Operations

        /// <summary>
        ///     Starts logging.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Start()
        {
            lock (this)
            {
                // Fail if logging has already been started
                if (LoggerState != State.Stopped)
                    return false;

                // Fail if the log file isn't specified
                if (string.IsNullOrEmpty(_logFilename))
                    return false;

                // Delete log file if it exists
                if (!_bAppend)
                {
                    try
                    {
                        File.Delete(_logFilename);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                // Open file for writing - return on error
                if (!File.Exists(_logFilename))
                {
                    try
                    {
                        _logFile = File.CreateText(_logFilename);
                    }
                    catch (Exception)
                    {
                        _logFile = null;
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        _logFile = File.AppendText(_logFilename);
                    }
                    catch (Exception)
                    {
                        _logFile = null;
                        return false;
                    }
                }
                _logFile.AutoFlush = true;

                // Return successfully
                _state = State.Running;
                return true;
            }
        }

        /// <summary>
        ///     Temporarily suspends logging.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Pause()
        {
            lock (this)
            {
                // Fail if logging hasn't been started
                if (LoggerState != State.Running)
                    return false;

                // Pause the logger
                _state = State.Paused;
                return true;
            }
        }

        /// <summary>
        ///     Resumes logging.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Resume()
        {
            lock (this)
            {
                // Fail if logging hasn't been paused
                if (LoggerState != State.Paused)
                    return false;

                // Resume logging
                _state = State.Running;
                return true;
            }
        }

        /// <summary>
        ///     Stops logging.
        /// </summary>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Stop()
        {
            lock (this)
            {
                // Fail if logging hasn't been started
                if (LoggerState != State.Running)
                    return false;

                // Stop logging
                try
                {
                    _logFile.Close();
                    _logFile = null;
                }
                catch (Exception)
                {
                    return false;
                }
                _state = State.Stopped;
                return true;
            }
        }

        /// <summary>
        ///     Logs a debug message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Debug
            (string msg)
        {
            _debugMsgs++;
            return WriteLogMsg(Level.Debug, msg);
        }

        /// <summary>
        ///     Logs an informational message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Info
            (string msg)
        {
            _infoMsgs++;
            return WriteLogMsg(Level.Info, msg);
        }

        /// <summary>
        /// Logs socket msg.
        /// </summary>
        /// <param name="ip">client ip</param>
        /// <param name="cmd">received command</param>
        /// <param name="msg">received message</param>
        /// <returns></returns>
        public bool Info(string ip, string cmd, string msg)
        {
            _infoMsgs++;
            string temp = string.Format("DEBUG:{0}-{1}-Ex-{2}", ip, cmd, msg);
            return WriteLogMsg(Level.Info, temp);
        }

        /// <summary>
        ///     Logs a success message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Success
            (string msg)
        {
            _successMsgs++;
            return WriteLogMsg(Level.Success, msg);
        }

        /// <summary>
        ///     Logs a warning message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Warning
            (string msg)
        {
            _warningMsgs++;
            return WriteLogMsg(Level.Warning, msg);
        }

        /// <summary>
        ///     Logs an error message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Error
            (string msg)
        {
            _errorMsgs++;
            return WriteLogMsg(Level.Error, msg);
        }

        /// <summary>
        ///     Logs a fatal error message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>true if successful, false otherwise.</returns>
        public bool Fatal
            (string msg)
        {
            _fatalMsgs++;
            return WriteLogMsg(Level.Fatal, msg);
        }

        /// <summary>
        ///     Retrieves the count of messages logged at one or more levels.
        /// </summary>
        /// <param name="levelMask">Mask indicating levels of interest.</param>
        /// <returns></returns>
        public uint GetMessageCount
            (uint levelMask)
        {
            uint uMessages = 0;
            if ((levelMask & (uint) Level.Debug) != 0)
                uMessages += _debugMsgs;
            if ((levelMask & (uint) Level.Info) != 0)
                uMessages += _infoMsgs;
            if ((levelMask & (uint) Level.Success) != 0)
                uMessages += _successMsgs;
            if ((levelMask & (uint) Level.Warning) != 0)
                uMessages += _warningMsgs;
            if ((levelMask & (uint) Level.Error) != 0)
                uMessages += _errorMsgs;
            if ((levelMask & (uint) Level.Fatal) != 0)
                uMessages += _fatalMsgs;
            return uMessages;
        }

        #endregion

        #region Fields

        /// <summary>Name of the log file.</summary>
        private readonly string _logFilename;

        /// <summary>Flag: append to existing file (if any).</summary>
        private readonly bool _bAppend;

        /// <summary>The log file.</summary>
        private StreamWriter _logFile;

        /// <summary>Levels to be logged.</summary>
        private uint _levels;

        /// <summary>The logger's state.</summary>
        private State _state = State.Stopped;

        /// <summary>Number of debug messages that have been logged.</summary>
        private uint _debugMsgs;

        /// <summary>Number of informational messages that have been logged.</summary>
        private uint _infoMsgs;

        /// <summary>Number of success messages that have been logged.</summary>
        private uint _successMsgs;

        /// <summary>Number of warning messages that have been logged.</summary>
        private uint _warningMsgs;

        /// <summary>Number of error messages that have been logged.</summary>
        private uint _errorMsgs;

        /// <summary>Number of fatal messages that have been logged.</summary>
        private uint _fatalMsgs;

        #endregion
    }
}