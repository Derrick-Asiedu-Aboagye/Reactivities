import { Box, Container, CssBaseline } from "@mui/material";
import { useEffect, useState } from "react";
import axios from "axios";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";

function App() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState<boolean>(false);

  useEffect(() => {
    axios.get<Activity[]>("https://localhost:5000/api/activities")
      .then(response => setActivities(response.data));
  }, []);

  const handleSelectActivity = (id: string) => {
    setSelectedActivity(activities.find(x => x.id === id));
  };

  const handleCancelSelectedActivity = () => {
    setSelectedActivity(undefined);
  };

  const handleOpenForm = (id?: string) => {
    if (id) handleSelectActivity(id);
    else handleCancelSelectedActivity();
    setEditMode(true);
  };

  const handleCloseForm = () => {
    setEditMode(false);
  };

  const handleSubmitForm = (activity: Activity) => {
    if (activity.id) {
      setSelectedActivity(activity);
      setActivities(activities.map(x => (
        x.id === activity.id ? activity : x 
      )))
    }
    else {
      const newActivity = {...activity, id: activities.length.toString()};
      setSelectedActivity(newActivity);
      setActivities([...activities, newActivity]);
    }
    setEditMode(false);
  }

  const handleDelete = (id: string) => {
    setActivities(activities.filter(activity =>
      activity.id !== id
    ));
  }

  return (
    <Box sx={{bgcolor: '#eeeeee'}}>
      <CssBaseline />
      <NavBar 
        openForm={handleOpenForm}
      />
      <Container maxWidth='xl' sx={{mt: 3}}>
        <ActivityDashboard 
          activities={activities}
          selectedActivity={selectedActivity}
          selectActivity={handleSelectActivity}
          cancelSelectedActivity={handleCancelSelectedActivity}
          editMode={editMode}
          openForm={handleOpenForm}
          closeForm={handleCloseForm}
          submitForm={handleSubmitForm}
          deleteActivity={handleDelete}
        />
      </Container>
    </Box>
  );
}

export default App;