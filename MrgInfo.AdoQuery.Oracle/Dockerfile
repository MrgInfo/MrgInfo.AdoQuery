FROM store/oracle/database-enterprise:12.2.0.1
ENV DB_PASSWD=P@ssword3...
RUN mkdir /home/oracle/dba
COPY --chown=oracle:oinstall database.sh database.sql /home/oracle/dba/
RUN chmod +x /home/oracle/dba/*.sh
HEALTHCHECK --interval=1m --timeout=30s --start-period=5m --retries=3 CMD /home/oracle/setup/healthcheck.sh
CMD /home/oracle/dba/database.sh
