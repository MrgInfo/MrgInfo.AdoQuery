FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu
ENV ACCEPT_EULA=Y SA_PASSWORD=P@ssword3...
RUN mkdir /dba
COPY database.sh database.sql healthcheck.sh /dba/
RUN chmod +x /dba/*.sh
HEALTHCHECK --interval=1m --timeout=30s --start-period=5m --retries=3 CMD /dba/healthcheck.sh
CMD /dba/database.sh
