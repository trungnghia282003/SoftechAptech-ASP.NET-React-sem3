import Button from '../../molecules/buttons/button'
import './user-information.scss'
import NavbarHeader from './menu'

const UserInformation = ({ text, handleDangXuat, maxIndex, role, handleCreateItem, currentUser, handleClick }) => {
  return (
    <div className='menu-information'>
      <NavbarHeader
        role={role}
        maxIndex={maxIndex}
        currentUser={currentUser}
        handleClick={handleClick}
        handleCreateItem={handleCreateItem}
      />
      <Button
        variant='light'
        className={{ 'image-logout': true }}
        onClick={handleDangXuat}
        text='Logout'
        iconPath='images/box-arrow-left.svg'
      />
      <div className='fullname-header mx-3'>
        Welcome <br />
        {text}!
      </div>
    </div>
  )
}

export default UserInformation
