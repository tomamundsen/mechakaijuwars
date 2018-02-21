echo "beginning deployment"
eb terminate
eb init -f
cd "/var/lib/jenkins/jobs/MKW Deploy Prod Platform/workspace"
git pull origin master
cd "/var/lib/jenkins/jobs/MKW Deploy Prod Platform/workspace/platform/app"
git add .
git commit -m "automated deployment by elastic beanstalk"
git aws.push
eb deploy
eb status --verbose
echo "finished deployment"
