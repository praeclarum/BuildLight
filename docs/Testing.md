# Testing

## Mac

Open a terminal

**Failed**  
`curl  http://buildlight.local/color?r=255&g=0&b=0`

**Success**  
`curl  http://buildlight.local/color?r=0&g=255&b=0`

**Reset**  
`curl  http://buildlight.local/color?r=255&g=255&b=0`

## Windows

Open [Windows Terminal](https://github.com/microsoft/terminal)

**Failed**  
`iwr -method POST -uri http://buildlight.local/color?r=255&g=0&b=0`

**Success**  
`iwr -method POST -uri http://buildlight.local/color?r=0&g=255&b=0`

**Reset**  
`iwr -method POST -uri http://buildlight.local/color?r=255&g=255&b=0`
