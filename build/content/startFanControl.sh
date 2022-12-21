#! /bin/bash

#/etc/systemd/system/startFanControl.service
#sudo systemctl enable startFanControl.service
#sudo systemctl start startFanControl.service
#sudo systemctl stop startFanControl.service
#sudo systemctl disable startFanControl.service
#systemctl status startFanControl.service
#sudo systemctl daemon-reload

[Unit]
Description=Stop/start FanControl

[Service]
User=root
ExecStart=/usr/local/sbin/FanControl Auto 17 54 46
ExecStop=pkill /usr/local/sbin/FanContro

[Install]
WantedBy=multi-user.target