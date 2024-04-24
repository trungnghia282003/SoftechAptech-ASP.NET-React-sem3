import constant from '../common/constant'

const notification = {
  getAllNotificationsByIdUser: async (id, token) => {
    const response = await fetch(`${constant.apiDomain}/Notification/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    })

    if (response.ok) {
      const content = await response.json()
      return content
    } else {
      throw new Error('Failed to found notification')
    }
  },

  updateNotification: async (token) => {
    const response = await fetch(`${constant.apiDomain}/Notification/update`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    })

    if (response.ok) {
      const content = await response.json()
      return content
    } else {
      throw new Error('Failed to found notification')
    }
  },
}
export default notification
