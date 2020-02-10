FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu
ENV ACCEPT_EULA=Y SA_PASSWORD=P@ssword3...
RUN mkdir /root/dba
COPY --chown=root:root database.sh healthcheck.sh database.sql /root/dba/
RUN chmod +x /root/dba/*.sh
HEALTHCHECK --interval=1m --timeout=30s --start-period=5m --retries=3 CMD /root/dba/healthcheck.sh
CMD /root/dba/database.sh
