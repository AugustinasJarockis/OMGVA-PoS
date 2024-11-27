FROM mcr.microsoft.com/mssql/server:2019-latest

# Switch to root user for access to apt-get install
USER root
RUN apt-get -y update  && \
        apt-get install -y curl && \
        apt-get install -y dos2unix && \
        apt-get install -y msodbcsql18

COPY . ./
RUN dos2unix *
RUN chmod +x init-scripts/init-db.sh

# Switch back to mssql user and run the entrypoint script
USER mssql
ENTRYPOINT ["./entrypoint.sh"]