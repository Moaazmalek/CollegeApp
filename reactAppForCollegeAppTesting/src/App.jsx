
// const App = () => {
//   const getAllStudents = async () => {
//     const res=await fetch('https://localhost:7096/api/student/all')
//     const data=await res.json()
//     console.log(data)
//     }
   
//   return (
//     <div>
//       <h1>Student Management System</h1>
//       <button onClick={getAllStudents}>Get All Students</button>

//     </div>
//   )
// }

// export default App
import React, { useState } from 'react'

const LoginComponent = () => {
  const [formData, setFormData] = useState({
    userName: '',
    password: ''
  })
  const [students, setStudents] = useState([])
  const login=async(formData)=>{
    try{
      const response=await fetch('https://localhost:7096/api/login',{
        method:'POST',
        headers:{
          'Content-Type':'application/json'
        },
        body:JSON.stringify(formData)
      })
      if(!response.ok){
        throw new Error('Login failed')
      }
      const data=await response.json()
      console.log('Login successful:',data)
    }catch(error){
      console.error('Error during login:',error)
    }
  }
  const OnSubmit = (e) => {
    e.preventDefault()
    login(formData)
  }
  const getAllStudents = async () => {
    const res = await fetch('https://localhost:7096/api/student/all')
    const data = await res.json()
    console.log(data)
    setStudents(data)
  }
  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={OnSubmit}>
        <div>
          <label>Username:</label>
          <input
            type="text"
            value={formData.username}
            onChange={(e) =>
              setFormData({ ...formData, username: e.target.value })
            }
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            value={formData.password}
            onChange={(e) =>
              setFormData({ ...formData, password: e.target.value })
            }
          />
        </div>
        <button type="submit">Login</button>
      </form>
      <button onClick={getAllStudents}>Get All Students</button>
   
      {JSON.stringify(students)}
    </div>
  )
}

export default LoginComponent