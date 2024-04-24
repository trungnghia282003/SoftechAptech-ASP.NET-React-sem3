import Login from './components/pages/login'
import Register from './components/pages/register'
import CategoryManagement from './components/pages/category-management'
import Home from './components/pages/home'
import MyBid from './components/pages/my-bid'
import BidDetail from './components/pages/bid-detail'
import Notification from './components/pages/notification'
import UserManagement from './components/pages/user-management'
import Profile from './components/pages/profile'
import CategoryBrowser from './components/pages/category-browser'
import SearchResult from './components/pages/search-results'
import ReportItems from './components/pages/report-items'
import ReportRating from './components/pages/report-rating'
import ForgotPassword from './components/pages/forgot-password'

const AppRoutes = [
  {
    index: true,
    path: '/',
    element: <Home />,
    name: 'home',
    title: 'Home page',
    icon: 'images/iconnav/danh_muc.png',
    roles: [0, 1],
  },
  {
    index: true,
    path: '/category-browser',
    element: <CategoryBrowser />,
    name: 'category-browser',
    roles: [0, 1],
  },
  {
    path: '/login',
    element: <Login />,
    name: 'login',
  },
  {
    path: '/register',
    element: <Register />,
    name: 'register',
  },
  {
    path: '/forgot-password',
    element: <ForgotPassword />,
    name: 'forgot-password',
  },
  {
    path: '/category-management',
    element: <CategoryManagement />,
    name: 'category-management',
    title: 'Category Management',
    icon: 'images/list-solid.svg',
    isMenu: true,
    roles: [2],
  },
  {
    path: '/my-bid',
    element: <MyBid />,
    name: 'my-bid',
    title: 'My Bidder',
    icon: 'images/heart-solid.svg',
    isMenu: true,
    roles: [0, 1],
  },
  {
    path: '/bid-detail',
    element: <BidDetail />,
    name: 'bid-detail',
    roles: [0, 1],
  },
  {
    path: '/search-results',
    element: <SearchResult />,
    name: 'search-results',
    roles: [0, 1],
  },
  {
    path: '/notification',
    element: <Notification />,
    name: 'notification',
    title: 'Notifications',
    icon: 'images/bell-solid.svg',
    isMenu: true,
    roles: [0, 1],
  },
  {
    path: '/user-management',
    element: <UserManagement />,
    name: 'Manage User',
    title: 'User Management',
    icon: 'images/iconnav/person-circle.svg',
    isMenu: true,
    roles: [2],
  },
  {
    path: '/profile',
    element: <Profile />,
    name: 'profile',
    title: 'My Profile',
    icon: 'images/user-solid.svg',
    isMenu: true,
    roles: [0, 1],
  },
  {
    path: '/report-items',
    element: <ReportItems />,
    name: 'report-items',
    title: 'Report Items',
    icon: 'images/export.svg',
    isMenu: true,
    roles: [1],
  },
  {
    path: '/report-ratings',
    element: <ReportRating />,
    name: 'report-ratings',
    title: 'User Leaderboard',
    icon: 'images/chart.svg',
    isMenu: true,
    roles: [0, 1, 2],
  },
]

export default AppRoutes
